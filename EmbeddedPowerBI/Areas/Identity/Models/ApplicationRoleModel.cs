using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbeddedPowerBI.Areas.Identity.Models
{
    public class ApplicationRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsAssigned { get; set; }
    }
}
