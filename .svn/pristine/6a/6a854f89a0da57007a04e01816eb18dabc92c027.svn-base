using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EmbeddedPowerBI.Areas.Identity.Services;
using EmbeddedPowerBI.Models;
using EmbeddedPowerBI.Services.Mail;

namespace EmbeddedPowerBI.Areas.Identity.Data
{
    public class SeedIdentityDb
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var smtpOptions = serviceProvider.GetService<IOptions<SmtpSettings>>();
            SmtpSettings settings = smtpOptions.Value;

            var passwordGenerator = serviceProvider.GetRequiredService<IPasswordGenerator>();
            var signInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();


            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                await EnsureOrganizationAsync(context);
                await EnsureRolesAsync(serviceProvider, RoleName.SysAdmin, RoleName.Admin, RoleName.User);
                await EnsureAdminUserAsync(serviceProvider, settings.SmtpUsername, passwordGenerator.GeneratePassword(signInManager), RoleName.SysAdmin);
            }
        }

        private static async Task EnsureOrganizationAsync(ApplicationDbContext context)
        {
            if (!context.Organizations.Any())
            {
                context.Organizations.Add(new Organization { Name = "IS" });
                await context.SaveChangesAsync();
            }
        }
        private static async Task EnsureRolesAsync(IServiceProvider serviceProvider, params string[] roles)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            foreach (string role in roles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }
                await roleManager.CreateAsync(new ApplicationRole(role));
            }
        }

        private static async Task EnsureAdminUserAsync(IServiceProvider serviceProvider, string userName, string password, params string[] roles)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user = await userManager.FindByNameAsync(userName);

            if (ReferenceEquals(user, null))
            {
                user = new ApplicationUser { UserName = userName, Email = userName, DisplayName = RoleName.SysAdmin, OrganizationID = 1, EmailConfirmed = true };
                await userManager.CreateAsync(user, password);
                await userManager.RemoveFromRolesAsync(user, roles);
                await userManager.AddToRolesAsync(user, roles);
            }

        }
    }
}
