using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarRent.Models;
using CarRent.Models.CarRentModels;

namespace CarRent.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //builder.Entity<RentModel>().ToTable("Rents");
            //builder.Entity<CarModel>().ToTable("Cars");
            //builder.Entity<SiteModel>().ToTable("Sites");
            //builder.Entity<ImageModel>().ToTable("Images");
        }

        public DbSet<RentModel> Rents { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<SiteModel> Sites { get; set; }
        public DbSet<ImageModel> Images { get; set; }
    }
}
