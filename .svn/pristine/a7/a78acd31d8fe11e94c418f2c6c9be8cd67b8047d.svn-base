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
    public class OrganizationsModel : PageModel
    {
        private readonly ILogger<OrganizationsModel> _logger;

        private readonly ApplicationDbContext context;

        public OrganizationsModel(ApplicationDbContext context, ILogger<OrganizationsModel> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [BindProperty]
        public List<Organization> Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            Input = await context.Organizations.ToListAsync();
        }

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
