using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DushinWebApp.Models;

namespace DushinWebApp.Services
{
    public class MyDbContext:IdentityDbContext
    {
        public DbSet<Location> LocationTbl { get; set; }
        public DbSet<Package> PackageTbl { get; set; }
        public DbSet<Profile> ProfileTbl { get; set; }
        public DbSet<Order> OrderTbl { get; set; }
        public DbSet<ProviderProfile> ProviderProfileTbl { get; set; }
        public DbSet<Feedback> FeedbackTbl { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=DushinTravelDb; Trusted_Connection=True");
            options.UseSqlServer(@"Server=tcp:dushinwebapp.database.windows.net,1433;Initial Catalog=TravelDb;Persist Security Info=False;User ID=dushinadmin;Password=cjt20uCj;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
