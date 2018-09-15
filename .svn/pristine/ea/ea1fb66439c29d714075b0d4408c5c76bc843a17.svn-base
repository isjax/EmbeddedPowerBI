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
using WebApplication4.Areas.Identity.Data;
using WebApplication4.Areas.Identity.Models;
using WebApplication4.Models;

namespace WebApplication4.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterOrganizationModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<RegisterOrganizationModel> logger;

        public RegisterOrganizationModel(
            ApplicationDbContext context,
            ILogger<RegisterOrganizationModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public Organization Input { get; set; }

        public string ReturnUrl { get; set; }

        public IActionResult OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                Organization organization = new Organization { Name = Input.Name };
                context.Organizations.Add(organization);
                var result = await context.SaveChangesAsync();
                logger.LogInformation("User created a new account with password.");
                return LocalRedirect(returnUrl);
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
