using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using EmbeddedPowerBI.Areas.Identity.Data;
using EmbeddedPowerBI.Services.Mail;
using EmbeddedPowerBI.Models;
using EmbeddedPowerBI.Areas.Identity.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace EmbeddedPowerBI
{
    public interface IUsersInitializer
    {
        Task Initialize();
    }

    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<PBISettings>(Configuration.GetSection("PBISettings"));
            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            services.AddIdentity<ApplicationUser, ApplicationRole>(o =>
             {
                 o.SignIn.RequireConfirmedEmail = false;
                 o.SignIn.RequireConfirmedPhoneNumber = false;
             })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.ExpireTimeSpan = new TimeSpan(0, 10, 0);
                options.SlidingExpiration = true;
            });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(RoleName.SysAdmin, policy => policy.RequireRole(RoleName.SysAdmin));
                config.AddPolicy(RoleName.Admin, policy => policy.RequireRole(RoleName.Admin));
            });

            //services.AddScoped<IUsersInitializer, UsersInitializer>();
            services.AddScoped<IPasswordGenerator, PasswordGenerator>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc(
                config =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    config.Filters.Add(new AuthorizeFilter(policy));

                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas")),
            //    RequestPath = "/Content"
            //});

            app.UseAuthentication();
            //userContextInitializer.Initialize();

            app.UseCookiePolicy();

            app.UseMvc();
        }


        //public struct RoleName
        //{
        //    public const string SysAdmin = "SysAdmin";
        //    public const string Admin = "Admin";
        //    public const string User = "User";
        //}
    }
    public struct RoleName
    {
        public const string SysAdmin = "System";
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
