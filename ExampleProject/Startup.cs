using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using ExampleProject.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using ExampleProject.BackGroundJob;
using Microsoft.Extensions.Logging;
using ExampleProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using System.IO;
using ExampleProject.UnitOfWork;
//using ExampleProject.Middleware;

namespace ExampleProject
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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddDbContext<DbDataContext>(options =>
       options.UseSqlServer(Configuration.GetConnectionString("DataConnection")));
            services.AddHangfire(_ => _.UseSqlServerStorage(Configuration.GetConnectionString("DataConnection")));
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<ICheckUrlService, CheckUrlService>();
            services.AddScoped<IMonitoringTaskService, MonitoringTaskService>();
            services.AddScoped<INotifacationService, NotifacationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider
            )
        {
          //  app.UseMiddleware<AuthenticationMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/html";

                        await context.Response.WriteAsync("<html lang=\"en\"><body>\r\n");
                        await context.Response.WriteAsync("ERROR!<br><br>\r\n");

                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();

                        var logger = context.RequestServices.GetService<ILogger<Startup>>();

                        logger.LogError(exceptionHandlerPathFeature?.Error, "unhandled exception occured");

                        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
                        {
                            await context.Response.WriteAsync(
                                                      "File error thrown!<br><br>\r\n");
                        }

                        await context.Response.WriteAsync(
                                                      "<a href=\"/\">Home</a><br>\r\n");
                        await context.Response.WriteAsync("</body></html>\r\n");
                        await context.Response.WriteAsync(new string(' ', 512));
                    });
                });
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            ConfigureHangfire(app, serviceProvider);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Url}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private static void ConfigureHangfire(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            // Configure hangfire to use the new JobActivator we defined.
            GlobalConfiguration.Configuration
                .UseActivator(new HangfireActivator(serviceProvider));

            // The rest of the hangfire config as usual.
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
