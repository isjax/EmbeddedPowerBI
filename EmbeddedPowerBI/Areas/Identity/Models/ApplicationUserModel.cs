using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmbeddedPowerBI.Areas.Identity.Data;

namespace EmbeddedPowerBI.Areas.Identity.Models
{
    public class ApplicationUserModel
    {
        //[Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Organization")]
        public int OrganizationID { get; set; }

        public List<ApplicationRole> Roles { get; set; }
    }
}
