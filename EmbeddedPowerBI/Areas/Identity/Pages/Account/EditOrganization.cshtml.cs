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
    public class EditOrganizationModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<EditOrganizationModel> logger;

        public EditOrganizationModel(
            ApplicationDbContext context,
            ILogger<EditOrganizationModel> logger)
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
                organization.Name = Input.Name;
                context.Organizations.Update(organization);
                var result = await context.SaveChangesAsync();
                logger.LogInformation("Organization name updated.");
                return LocalRedirect(returnUrl);
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
