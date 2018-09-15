using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Areas.Identity.Models;
using EmbeddedPowerBI.Areas.Identity.Pages.Account.Manage;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]

    public class EditUserModel : PageModel
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext context;
        private ILogger logger;

        public EditUserModel(UserManager<ApplicationUser> userManager, ILogger<EditUserModel> logger, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? "/";

            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{(string.IsNullOrEmpty(id) ? userManager.GetUserId(User) : id)}'.");
            }

            //Get organizations
            var organizations = await context.Organizations.ToListAsync();
            //Get roles
            List<ApplicationRole> userRoles = await context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role)
                .ToListAsync();
            List<ApplicationRoleModel> roles = await userManager.IsInRoleAsync(user, RoleName.Admin) ?
                await context.Roles
                    .Where(r => r.Name != RoleName.SysAdmin)
                    .Select(r => new ApplicationRoleModel { Id = r.Id, Name = r.Name, IsAssigned = userRoles.Any(ur => ur.Id == r.Id) })
                    .ToListAsync() :
                 await context.Roles
                    .Select(r => new ApplicationRoleModel { Id = r.Id, Name = r.Name, IsAssigned = userRoles.Any(ur => ur.Id == r.Id) })
                    .ToListAsync();

            Input = new InputModel
            {
                User = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = userRoles,
                    Name = user.DisplayName,
                    OrganizationID = user.OrganizationID,
                },
                Organizations = organizations,
                Roles = roles
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByIdAsync(Input.User.Id);
                user.Email = Input.User.Email;
                user.UserName = Input.User.Email;
                user.DisplayName = Input.User.Name;
                user.OrganizationID = Input.User.OrganizationID;

                var removeFromRoles = await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
                var addToRoles = await userManager.AddToRolesAsync(user, Input.Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray());

                var updateUser = await userManager.UpdateAsync(user);

                if (addToRoles.Succeeded && removeFromRoles.Succeeded && updateUser.Succeeded)
                {
                    logger.LogInformation("User account updated.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in addToRoles.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in removeFromRoles.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                foreach (var error in updateUser.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            //public class UserData
            //{
            //    [Required]
            //    [Display(Name = "Email")]
            //    public string Email { get; set; }

            //    [Required]
            //    [Display(Name = "User")]
            //    public string Name { get; set; }

            //    [Required]
            //    [Display(Name = "Organization")]
            //    public int OrganizationID { get; set; }
            //    public List<ApplicationRole> Roles { get; set; }
            //}

            public ApplicationUserModel User { get; set; }
            public List<ApplicationRoleModel> Roles { get; set; }
            public List<Organization> Organizations { get; set; }
        }
    }
}