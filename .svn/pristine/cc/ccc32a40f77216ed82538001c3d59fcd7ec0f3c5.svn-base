using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmbeddedPowerBI.Models;

namespace EmbeddedPowerBI.Areas.Identity.Data
{
    public static class DbInitializer
    {
        public static void Initialize (ApplicationDbContext context)
        {
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            if (!context.Organizations.Any())
            {
                context.Organizations.Add(new Organization { Name = "IS" });
                context.Organizations.Add(new Organization { Name = "DLA" });
                context.Organizations.Add(new Organization { Name = "Madura" });
                context.SaveChanges();
            }
        }
    }
}
