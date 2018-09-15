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

    public class DeleteUserModel : PageModel
    {
        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext context;
        private ILogger logger;

        public DeleteUserModel(UserManager<ApplicationUser> userManager, ILogger<DeleteUserModel> logger, ApplicationDbContext context)
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
            var organization = await context.Organizations.FirstOrDefaultAsync(org => org.Id == user.OrganizationID);
            //Get roles
            List<string> roles = await context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            Input = new InputModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.DisplayName,
                Organization = organization.Name,
                Roles = roles
            };

            return Page();
        }

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        //    }

        //    RequirePassword = await _userManager.HasPasswordAsync(user);
        //    if (RequirePassword)
        //    {
        //        if (!await _userManager.CheckPasswordAsync(user, Input.Password))
        //        {
        //            ModelState.AddModelError(string.Empty, "Password not correct.");
        //            return Page();
        //        }
        //    }

        //    var result = await _userManager.DeleteAsync(user);
        //    var userId = await _userManager.GetUserIdAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{userId}'.");
        //    }

        //    await _signInManager.SignOutAsync();

        //    _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        //    return Redirect("~/");
        //}

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {

                var user = await userManager.FindByIdAsync(Input.Id);
                var result = userManager.DeleteAsync(user);
                //var removeFromRoles = await userManager.RemoveFromRolesAsync(user, await userManager.GetRolesAsync(user));
                if (result.Result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    throw new InvalidOperationException($"Unexpected error occurred deleteing user with ID '{user.Id}'.");
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {

            public string Id { get; set; }

            [Display(Name = "User")]
            public string Name { get; set; }

            [Display(Name = "Email")]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Display(Name = "Organization")]
            public string Organization { get; set; }

            public List<string> Roles { get; set; }
        }
    }
}