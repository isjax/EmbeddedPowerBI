using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Organizations
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<EditModel> logger;

        public DeleteModel(
            ApplicationDbContext context,
            ILogger<EditModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public Organization Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            Input = await context.Organizations.FirstOrDefaultAsync(org => org.Id == id);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                Organization organization = await context.Organizations.FirstOrDefaultAsync(org => org.Id == Input.Id);
                context.Organizations.Remove(organization);
                var result = await context.SaveChangesAsync();
                logger.LogInformation("Organization removed.");
                return LocalRedirect(returnUrl);
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}