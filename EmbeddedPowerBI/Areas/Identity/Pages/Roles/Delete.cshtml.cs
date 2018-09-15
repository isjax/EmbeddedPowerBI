using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private RoleManager<ApplicationRole> roleManager;
        private readonly ApplicationDbContext context;
        private readonly ILogger<EditModel> logger;

        public DeleteModel(RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext context,
            ILogger<EditModel> logger)
        {
            this.roleManager = roleManager;
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public ApplicationRole Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            Input = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                ApplicationRole role = await roleManager.Roles.FirstOrDefaultAsync(org => org.Id == Input.Id);
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    logger.LogInformation("Role deleted.");
                    return LocalRedirect(returnUrl);
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}