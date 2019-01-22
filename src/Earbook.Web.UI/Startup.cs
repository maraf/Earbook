using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Earbook.Models;
using Earbook.Models.Options;
using Earbook.Models.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Earbook
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.Configure<Storage>(Configuration.GetSection("Storage"));

            var connectionStrings = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(connectionStrings.Data)
            );

            using (var provider = services.BuildServiceProvider())
            {
                var db = provider.GetService<DataContext>();
                db.Database.EnsureCreated();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }

    public static class ApplicationDataSeeder
    {
        public static IWebHost SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var db = services.GetService<DataContext>();
                db.Database.EnsureCreated();
            }

            return host;
        }
    }
}
