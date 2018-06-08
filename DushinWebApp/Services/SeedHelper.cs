using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DushinWebApp.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DushinWebApp.Services
{
    public static class SeedHelper
    {
        public static async Task Seed(IServiceProvider provider)
        {
            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            //profile service
            DataService<Profile> profileService = new DataService<Profile>();
            DataService<ProviderProfile> providerProfileService = new DataService<ProviderProfile>();

            using (var scope = scopeFactory.CreateScope())
            {
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //add Customer role
                if (await roleManager.FindByNameAsync("Customer") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Customer"));
                }
                //add Admin role
                if (await roleManager.FindByNameAsync("Admin") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                //add Provider role
                if (await roleManager.FindByNameAsync("Provider") == null)
                {
                    await roleManager.CreateAsync(new IdentityRole("Provider"));
                }

                //add default Provider
                if (await userManager.FindByNameAsync("Admin") == null)
                {
                    IdentityUser user = new IdentityUser("Admin");
                    await userManager.CreateAsync(user, "Admin3###");
                    await userManager.AddToRoleAsync(user, "Admin");
                }

                //add default Provider
                if (await userManager.FindByNameAsync("Provider") == null)
                {
                    IdentityUser user = new IdentityUser("John");
                    await userManager.CreateAsync(user, "John3###");
                    await userManager.AddToRoleAsync(user, "Provider");
                    //add a default profile for this admin
                    ProviderProfile profile = new ProviderProfile
                    {
                        UserId = user.Id
                    };
                    providerProfileService.Create(profile);
                }
            }
        }
    }
}
