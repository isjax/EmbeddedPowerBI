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
using EmbeddedPowerBI.Areas.Identity.Pages.Account;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Pages.Organizations
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ILogger<OrganizationsModel> logger;

        private readonly ApplicationDbContext context;

        public IndexModel(ApplicationDbContext context, ILogger<OrganizationsModel> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [BindProperty]
        public List<Organization> Input { get; set; }

        public string ReturnUrl { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            Input = await context.Organizations.ToListAsync();
        }
    }
}
