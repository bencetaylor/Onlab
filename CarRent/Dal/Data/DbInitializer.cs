using CarRent.DAL.Models.CarRentModels;
using System.Linq;

namespace CarRent.DAL.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            /*
            var UserManager = new UserManager<ApplicationUser>(context);
            var roleManager = new RoleManager<IdentityRole<int>>();
            //new UserStore<ApplicationUser>(context)
            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "admin@gmail.com";

                string userPWD = "Ab-1234";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Regular"))
            {
                var role = new IdentityRole();
                role.Name = "Regular";
                roleManager.Create(role);

            }
            */

            if (context.Cars.Any())
            {
                return;
            }

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
