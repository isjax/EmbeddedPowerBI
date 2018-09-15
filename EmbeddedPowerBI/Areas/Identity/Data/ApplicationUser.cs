using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EmbeddedPowerBI.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the User class
    public class ApplicationUser : IdentityUser
    {
        public int OrganizationID { get; set; }
        public string DisplayName { get; set; }
        public Organization Organization { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
