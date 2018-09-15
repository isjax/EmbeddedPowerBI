using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Areas.Identity.Models;
using EmbeddedPowerBI.Areas.Identity.Services;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IPasswordGenerator passwordGenerator;

        public RegisterModel(IEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IPasswordGenerator passwordGenerator,
            ILogger<RegisterModel> logger)
        {
            this.context = context;
            this.emailSender = emailSender;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passwordGenerator = passwordGenerator;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            //[Required]
            //[EmailAddress]
            //[Display(Name = "Email")]
            //public string Email { get; set; }

            public ApplicationUserModel User { get; set; }
            public List<ApplicationRoleModel> Roles { get; set; }
            public List<Organization> Organizations { get; set; }

            //[Required]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Password")]
            //public string Password { get; set; }

            //[DataType(DataType.Password)]
            //[Display(Name = "Confirm password")]
            //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            //public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            var user = await userManager.GetUserAsync(User);

            Input = new InputModel
            {
                User = new ApplicationUserModel(),
            };

            //Get roles
            var roles = context.Roles
                .Select(r => new ApplicationRoleModel { Id = r.Id, Name = r.Name });

            if (await userManager.IsInRoleAsync(user, RoleName.Admin))
            {
                Input.Organizations = await context.Organizations.Where(org => org.Id == user.OrganizationID).ToListAsync();
                roles = roles.Where(r => r.Name != RoleName.SysAdmin).OrderBy(r => r.Name);
                Input.Roles = await roles.ToListAsync();

            }
            else
            {
                Input.Organizations = await context.Organizations.ToListAsync();
                Input.Roles = await roles.ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                string password = passwordGenerator.GeneratePassword(signInManager);
                var user = new ApplicationUser { UserName = Input.User.Email, Email = Input.User.Email, DisplayName = Input.User.Name, OrganizationID = Input.User.OrganizationID, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    logger.LogInformation("User created a new account with password.");
                    var addToRoles = await userManager.AddToRolesAsync(user, Input.Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray());


                    var code = await userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { code },
                        protocol: Request.Scheme);

                    await emailSender.SendEmailAsync(Input.User.Email, "You have been registered for WISARD",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
