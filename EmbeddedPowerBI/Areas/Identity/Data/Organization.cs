using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmbeddedPowerBI.Areas.Identity.Data
{
    public class Organization
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100,ErrorMessage ="Name may be up to 100 characters long.")]
        public string Name { get; set; }
    }
}
