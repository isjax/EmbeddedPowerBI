using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class UsersModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<UsersModel> logger;
        //private readonly IEmailSender _emailSender;

        private readonly ApplicationDbContext usersContext;

        public UsersModel(
            ApplicationDbContext usersContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UsersModel> logger)
        {
            this.usersContext = usersContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        [BindProperty]
        public List<InputModel> Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            public string ID { get; set; }

            [Display(Name = "User")]
            public string UserName { get; set; }

            [Display(Name = "Email")]
            public string Email { get; set; }

            [Display(Name = "Organization")]
            public Organization Organization { get; set; }

            [Display(Name = "Roles")]
            public string[] Roles { get; set; }
        }

        public async Task OnGetAsync()
        {
            var tmp = await userManager.Users
                  .Include(u => u.Organization)
                  .Include(u => u.UserRoles)
                      .ThenInclude(ur => ur.Role)
                  .AsNoTracking()
                  .ToListAsync();

            var user = await userManager.GetUserAsync(User);

            var input = userManager.Users
                      .Include(u => u.Organization)
                      .Include(u => u.UserRoles)
                          .ThenInclude(ur => ur.Role)
                      .AsNoTracking()
                      .Select(u => new InputModel
                      {
                          ID = u.Id,
                          Email = u.Email,
                          Roles = u.UserRoles.Select(ur => ur.Role.Name).ToArray(),
                          UserName = u.DisplayName,
                          Organization = u.Organization,
                      });

            if (await userManager.IsInRoleAsync(user, RoleName.Admin))
            {
                input = input.Where(u => u.Organization.Id == user.OrganizationID);
            }

            Input = await input.ToListAsync();
        }
        //public void OnGet(string returnUrl = null)
        //{
        //    ReturnUrl = returnUrl;
        //}

        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl = returnUrl ?? Url.Content("~/");
        //    if (ModelState.IsValid)
        //    {
        //        var user = new User { UserName = Input.Email, Email = Input.Email };
        //        var result = await _userManager.CreateAsync(user, Input.Password);
        //        if (result.Succeeded)
        //        {
        //            _logger.LogInformation("User created a new account with password.");

        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { userId = user.Id, code = code },
        //                protocol: Request.Scheme);

        //            await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        //                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //            await _signInManager.SignInAsync(user, isPersistent: false);
        //            return LocalRedirect(returnUrl);
        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return Page();
        //}
    }
}
