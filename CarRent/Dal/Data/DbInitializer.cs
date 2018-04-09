using CarRent.DAL.Models;
using CarRent.DAL.Models.CarRentModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CarRent.DAL.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
            context.SaveChanges();


            var admin = new ApplicationUser
            {
                UserName = "bencetaylor@gmail.com",
                Email = "bencetaylor@gmail.com"
            };

            string userPWD = "Admin1234";

            userManager.CreateAsync(admin, userPWD);
            userManager.AddToRoleAsync(admin, roles[0].Id);

            var user = new ApplicationUser
            {
                UserName = "example@gmail.com",
                Email = "example@gmail.com"
            };

            userPWD = "User1234";

            userManager.CreateAsync(user, userPWD);
            userManager.AddToRoleAsync(user, roles[1].Id);
            

            var adminRole = new IdentityUserRole<string>()
            {
                RoleId = roles[0].Id,
                UserId = admin.Id
            };

            context.UserRoles.Add(adminRole);

            var userRole = new IdentityUserRole<string>()
            {
                RoleId = roles[1].Id,
                UserId = user.Id
            };

            context.UserRoles.Add(userRole);
            

            //// Csak egy mentés a végén, nehogy összeakadjon
            //context.SaveChanges();
            
            var site1 = new SiteModel { Name = "Site1", Address = "Address1" };
            var site2 = new SiteModel { Name = "Site2", Address = "Address2" };
            // Creating sites
            var sites = new SiteModel[]
            {
            site1, site2
            };
            foreach (SiteModel s in sites)
            {
                context.Sites.Add(s);
            }
            //context.SaveChanges();

            // Creating cars and place them in site1
            var cars = new CarModel[]
            {
            new CarModel{Brand="BMW 316i", NumberPlate="ABC123", Type=CarTypes.Types.Sedan, Price=55000, Consuption=4, Site=site1, Doors=4, Passangers=5, Power=150, Trunk=400, State=CarState.Available},
            new CarModel{Brand="BMW 330xd", NumberPlate="ABC321", Type=CarTypes.Types.Sedan, Price=80000, Site=site1},
            new CarModel{Brand="BMW 320d", NumberPlate="ABC456", Type=CarTypes.Types.Sedan, Price=60000, Site=site1},
            new CarModel{Brand="BMW 318d touring", NumberPlate="ABC789", Type=CarTypes.Types.Hatchback, Price=70000, Site=site1},
            };
            foreach (CarModel c in cars)
            {
                context.Cars.Add(c);
            }
            //context.SaveChanges();

            // Add cars to site1
            foreach (var c in context.Cars)
            {
                context.Sites.First().Cars.Add(c);
            }
            //context.SaveChanges();

            // Creating rents
            var rents = new RentModel[]
            {
                new RentModel{Car=cars[0],User=user,Site=site1,RentStarts=new System.DateTime(2018,3,15),RentEnds=new System.DateTime(2018,3,20),State=RentState.Pending}
            };

            foreach (RentModel r in rents)
            {
                context.Rents.Add(r);
            }
            context.SaveChanges();
        }
    }
}
