using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Models;

[assembly: HostingStartup(typeof(EmbeddedPowerBI.Areas.Identity.IdentityHostingStartup))]
namespace EmbeddedPowerBI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UsersContextConnection")));

                //services.AddDefaultIdentity<User>()
                //    .AddEntityFrameworkStores<UsersContext>();
            });
        }
    }
}