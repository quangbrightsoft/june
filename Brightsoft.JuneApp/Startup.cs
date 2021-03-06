using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Brightsoft.Authentication;
using Brightsoft.CronJob;
using Brightsoft.Data;
using Brightsoft.Data.Data;
using Brightsoft.Data.Entities;
using Brightsoft.Data.Identity.Accounts;
using Brightsoft.Data.Identity.Roles;
using Brightsoft.GraphQL.Helpers;
using Brightsoft.JuneApp.Cron;
using Brightsoft.JuneApp.GraphQl.QueryBuilders;
using Brightsoft.JuneApp.Services;
using GraphQL.Server.Ui.GraphiQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Brightsoft.JuneApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            // Configure Identity
            services.AddIdentity();
            services.AddRazorPages();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "adminfrontend/build";
            });


            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Remove all default claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Configure JWT Bearer Authentication
            services.AddJwtBearerAuthentication(Configuration.GetSection("Jwt"));


            // Add graph QL API
            services.AddGraphQlServices();

            services.AddApplicationServices();

            services.AddCronJob<MyCronJob1>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"*/1 * * * *";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env,
            ApplicationDbContext dbContext,
            RoleManager<Role> roleManager,
            UserManager<Account> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseGraphQlWithAuth();

            app.UseGraphiQLServer(new GraphiQLOptions
            {
                GraphQLEndPoint = "/api/graphql"
            });

            app.UseHttpsRedirection();

            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "adminfrontend";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            // Ensure database and tables are created
            var dbInitializer = new DbInitializer(dbContext, userManager, roleManager);
            dbInitializer.SeedData().Wait();
        }

    }
}
