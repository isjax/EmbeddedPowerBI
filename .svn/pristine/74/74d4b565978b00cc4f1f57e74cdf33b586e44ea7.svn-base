using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Roles
{
    [AllowAnonymous]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<EditModel> logger;

        public EditModel(
            ApplicationDbContext context,
            ILogger<EditModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public ApplicationRole Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            Input = await context.Roles.FirstOrDefaultAsync(org => org.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                ApplicationRole role = await context.Roles.FirstOrDefaultAsync(org => org.Id == Input.Id);
                role.Name = Input.Name;
                context.Roles.Update(role);
                var result = await context.SaveChangesAsync();
                logger.LogInformation("Role name updated.");
                return LocalRedirect(returnUrl);
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
