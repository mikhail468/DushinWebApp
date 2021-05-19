using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DushinWebApp.Models;
using DushinWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;

namespace DushinWebApp
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwaggerDocument();
            services.AddScoped<IDataService<Location>, DataService<Location>>();
            services.AddScoped<IDataService<Package>, DataService<Package>>();
            services.AddScoped<IDataService<Profile>, DataService<Profile>>();
            services.AddScoped<IDataService<Order>, DataService<Order>>();
            services.AddScoped<IDataService<ProviderProfile>, DataService<ProviderProfile>>();
            services.AddScoped<IDataService<Feedback>, DataService<Feedback>>();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddSingleton(x => new BlobServiceClient(_config.GetValue<string>("AzureBlobStorageConnectionString")));
            services.AddSingleton<IBlobService, BlobService>();

            services.AddIdentity<IdentityUser, IdentityRole>(config =>

            {
                config.Password.RequiredLength = 6;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            }
            ).AddEntityFrameworkStores<MyDbContext>();
            services.AddDbContext<MyDbContext>(options => {
                options.UseSqlServer(_config["TravelDBConString"]);
            });
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
            app.UseSwaggerUi3();

            //call seed
            // SeedHelper.Seed(app.ApplicationServices).Wait();
        }
    }
}
