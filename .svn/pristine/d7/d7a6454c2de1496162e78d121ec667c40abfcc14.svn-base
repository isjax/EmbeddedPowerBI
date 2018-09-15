using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmbeddedPowerBI.Areas.Identity.Data;

namespace EmbeddedPowerBI.Extensions
{
    public  static partial class Extensions
    {
        public static  async Task<string> GetUserDisplayNameAsync(this UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            var result = await userManager.FindByIdAsync(user.Identity.Name);
            return result.DisplayName;
        }
    }
}
