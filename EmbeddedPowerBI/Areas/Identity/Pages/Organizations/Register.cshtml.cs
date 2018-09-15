using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Organizations
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<RegisterModel> logger;

        public RegisterModel(ApplicationDbContext context, ILogger<RegisterModel> logger)
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
