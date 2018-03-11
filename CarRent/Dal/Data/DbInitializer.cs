using CarRent.DAL.Models;
using CarRent.DAL.Models.CarRentModels;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace CarRent.DAL.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            context.Database.EnsureCreated();
            
            if (context.Cars.Any())
            {
                return;
            }

            var roles = new IdentityRole[]
            {
                new IdentityRole{Id="admin", Name="ADMIN"},
                new IdentityRole{Id="user", Name="USER"}
            };
            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }


            var user = new ApplicationUser
            {
                UserName = "bencetaylor@gmail.com",
                Email = "bencetaylor@gmail.com"
            };
            
            string userPWD = "Admin1234";

            userManager.CreateAsync(user, userPWD);

            var cars = new CarModel[]
            {
            new CarModel{Brand="BMW 316i", NumberPlate="ABC123", Type="Sedan", Price=55000, Consuption=4, Doors=4, Passangers=5, Power=150, Trunk=400, State=CarState.Available},
            new CarModel{Brand="BMW 330xd", NumberPlate="ABC123", Type="Sedan", Price=80000},
            new CarModel{Brand="BMW 320d", NumberPlate="ABC123", Type="Sedan", Price=60000},
            new CarModel{Brand="BMW 318d touring", NumberPlate="ABC123", Type="Combi", Price=70000},
            };
            foreach (CarModel c in cars)
            {
                context.Cars.Add(c);
            }
            context.SaveChanges();

            var sites = new SiteModel[]
            {
            new SiteModel{Name="Site1", Address="Address1"},
            new SiteModel{Name="Site2", Address="Address2" },
            };
            foreach (SiteModel s in sites)
            {
                context.Sites.Add(s);
            }
            context.SaveChanges();

            var rents = new RentModel[]
            {
                new RentModel{}
            };

            foreach (RentModel r in rents)
            {
                context.Rents.Add(r);
            }
            context.SaveChanges();
        }
    }
}
