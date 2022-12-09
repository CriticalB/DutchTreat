using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using DutchTreat.Services;
using DutchTreat.Data;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DutchContext>(cfg =>
            {
                cfg.UseSqlServer();
            });
                
            services.AddTransient<DutchSeeder>();
            services.AddTransient<IMailService, NullMailService>();

            services.AddScoped<IDutchRepository, DutchRepository>();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }
    

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.MapGet("/", () => "Hello World!");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(cfg =>
            {
                cfg.MapRazorPages();

                cfg.MapControllerRoute("Default", 
                "/{controller}/{action}/{id?}",
                new { controller = "App", action = "Index"});
            });        
        }
    }
}

