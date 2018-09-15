using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Roles
{
    [AllowAnonymous]
    public class CreateModel : PageModel
    {
        private RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext context;
        private readonly ILogger<CreateModel> logger;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger, RoleManager<ApplicationRole> roleManager)
        {
            this.context = context;
            this.logger = logger;
            this.roleManager = roleManager;
        }
        [BindProperty]
        public ApplicationRole Input { get; set; }

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
                ApplicationRole role = new ApplicationRole { Name = Input.Name };
                var result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Role created.");
                    return LocalRedirect(returnUrl);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

    }
}
