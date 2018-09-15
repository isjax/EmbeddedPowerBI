using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Data
{

    public class UsersInitializer : IUsersInitializer
    {
        private ApplicationDbContext dbContext;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        public UsersInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.dbContext = dbContext;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task Initialize()
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            if (dbContext.Roles.Any()) return;

            DbInitializer.Initialize(dbContext);

            await roleManager.CreateAsync(new ApplicationRole(RoleName.SysAdmin));
            await roleManager.CreateAsync(new ApplicationRole(RoleName.Admin));
            await roleManager.CreateAsync(new ApplicationRole(RoleName.User));

            string email = "reuven.jackson@inherentsimplicity.com";
            var user = new ApplicationUser() { Email = email, UserName = email, DisplayName = "rjax", OrganizationID = 1 };
            IdentityResult result = await userManager.CreateAsync(user, "Abc?123");
            await userManager.AddToRoleAsync(await userManager.FindByEmailAsync(email), RoleName.SysAdmin);
        }
    }
}

