﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using DushinWebApp.Models;
using DushinWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DushinWebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IDataService<Location>, DataService<Location>>();
            services.AddScoped<IDataService<Package>, DataService<Package>>();
            services.AddScoped<IDataService<Profile>, DataService<Profile>>();
            services.AddScoped<IDataService<Order>, DataService<Order>>();
            services.AddScoped<IDataService<ProviderProfile>, DataService<ProviderProfile>>();
            services.AddScoped<IDataService<Feedback>, DataService<Feedback>>();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddIdentity<IdentityUser, IdentityRole>(config =>

            {
                config.Password.RequiredLength = 6;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            }
            ).AddEntityFrameworkStores<MyDbContext>();
            services.AddDbContext<MyDbContext>();
            services.ConfigureApplicationCookie(options =>
            { options.AccessDeniedPath = "/Account/Denied"; }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            //call seed
            //SeedHelper.Seed(app.ApplicationServices).Wait();
        }
    }
}
