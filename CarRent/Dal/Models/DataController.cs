using CarRent.DAL.Data;
using CarRent.DAL.Models.CarRentModels;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CarRent.DAL.Models
{
    public class DataController
    {
        public ApplicationDbContext context;

        public DataController()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-CarRent-DB5C1E85-9674-4136-B1DB-EFCD10B4A0C5;Trusted_Connection=True;MultipleActiveResultSets=true");
            
            context = new ApplicationDbContext(optionsBuilder.Options);
        }

        // Manage Cars
        public IList<CarDTO> GetCars()
        {
            var cars = context.Cars
                .Include( c => c.Site)
                .Include( c => c.Images)
                .Select(c => new CarDTO
                {
                    CarID = c.CarID,
                    Price = c.Price,
                    Type = c.Type,
                    Brand = c.Brand ?? "none",
                    Location = c.Site.Address,
                    NumberPlate = c.NumberPlate,
                    Image = c.Images.FirstOrDefault()
                }).ToList();
            
            return cars;
        }

        public IList<CarDTO> SearchCars(int[] searchValues)
        {
            var cars = context.Cars
                .Include(c => c.Site)
                .Include(c => c.Images)
                .Where(c => c.Price >= searchValues[0] && c.Price <= searchValues[1] && c.Passangers >= searchValues[2] && c.Passangers <= searchValues[3])
                .Select(c => new CarDTO
                {
                    CarID = c.CarID,
                    Price = c.Price,
                    Type = c.Type,
                    Brand = c.Brand ?? "none",
                    Location = c.Site.Address,
                    NumberPlate = c.NumberPlate,
                    Image = c.Images.FirstOrDefault()
                }).ToList();

            return cars;
        }

        public IList<CarDTO> GetTopCars()
        {
            var cars = context.Cars
                .Include(c => c.Site)
                .Include(c => c.Images)
                .Select(c => new CarDTO
                {
                    CarID = c.CarID,
                    Price = c.Price,
                    Type = c.Type,
                    Brand = c.Brand ?? "none",
                    Location = c.Site.Address,
                    NumberPlate = c.NumberPlate,
                    Image = c.Images.FirstOrDefault()
                }).Take(5)
                .ToList();

            return cars;
        }

        public IList<CarDetailsDTO> GetCarsDetailed()
        {
            var cars = context.Cars
                .Include(c => c.Site)
                //.Include(c => c.Images)
                //.Include( c => c.Comments)
                .Select(c => new CarDetailsDTO
                {
                    CarID = c.CarID,
                    Brand = c.Brand,
                    Location = c.Site,
                    NumberPlate = c.NumberPlate,
                    Passangers = c.Passangers,
                    Power = c.Power,
                    Price = c.Price,
                    Consuption = c.Consuption,
                    Doors = c.Doors,
                    State = c.State,
                    Trunk = c.Trunk,
                    Type = c.Type,
                    Description = c.Description,
                    //Images = c.Images,
                    //Comments = c.Comments
                }).ToList();

            return cars;
        }

        public IList<CarDetailsDTO> GetCarsWithFullDetail()
        {
            var cars = context.Cars
                .Include(c => c.Site)
                .Include(c => c.Images)
                .Include(c => c.Comments)
                .Select(c => new CarDetailsDTO
                {
                    CarID = c.CarID,
                    Brand = c.Brand,
                    Location = c.Site,
                    NumberPlate = c.NumberPlate,
                    Passangers = c.Passangers,
                    Power = c.Power,
                    Price = c.Price,
                    Consuption = c.Consuption,
                    Doors = c.Doors,
                    State = c.State,
                    Trunk = c.Trunk,
                    Type = c.Type,
                    Description = c.Description,
                    //Images = c.Images,
                    //Comments = c.Comments
                }).ToList();

            return cars;
        }

        public CarDetailsDTO GetCar(int id)
        {
            var car = context.Cars
                .Include(c => c.Site)
                .Include(c => c.Images)
                .Include(c => c.Comments)
                .Where(c => c.CarID == id)
                .Select(c => new CarDetailsDTO
                {
                    CarID = c.CarID,
                    Brand = c.Brand,
                    Location = c.Site,
                    NumberPlate = c.NumberPlate,
                    Passangers = c.Passangers,
                    Power = c.Power,
                    Price = c.Price,
                    Consuption = c.Consuption,
                    Doors = c.Doors,
                    State = c.State,
                    Trunk = c.Trunk,
                    Type = c.Type,
                    Description = c.Description,
                    Images = c.Images,
                    //Comments = c.Comments
                }).FirstOrDefault();
            return car;
        }

        public CarModel GetCar(string Numberplate)
        {
            return context.Cars.Find(Numberplate);
        }

        public CarModel FindCar(int id)
        {
            return context.Cars.Find(id);
        }

        public void DeleteCar(int id)
        {
            // Itt törölni kell a képeket és kommenteket is amik az autóhoz tartoznak
            var car = context.Cars.Find(id);

            var images = car.Images.ToList();
            if(images !=null && images.Count()>0)
            {
                context.Images.RemoveRange(images);
            }
            
            context.SaveChanges();

            foreach (var img in car.Images)
            {
                if (File.Exists(img.Path))
                {
                    File.Delete(img.Path);
                }
                context.Images.Remove(img);
                context.SaveChanges();
            }
            foreach (var comment in car.Comments)
            {
                context.Comments.Remove(comment);
            }

            context.Cars.Remove(car);

            context.SaveChanges();
        }

        public void CreateOrUpdateCar(CarDetailsDTO c, List<ImageDTO> images)
        {
            var site = c.Location;

            CarModel carModel;

            if (context.Cars.Any(car => car.CarID == c.CarID))
            {
                carModel = context.Cars.Find(c.CarID);
                carModel.Brand = c.Brand;
                carModel.CarID = c.CarID;
                //Comments = c.Comments;
                carModel.Consuption = c.Consuption;
                carModel.Description = c.Description;
                carModel.Doors = c.Doors;
                // TODO Location not working 
                carModel.Site = c.Location;
                carModel.NumberPlate = c.NumberPlate;
                carModel.Passangers = c.Passangers;
                carModel.Power = c.Power;
                carModel.Price = c.Price;
                carModel.State = c.State;
                carModel.Trunk = c.Trunk;
                carModel.Type = c.Type;
            }
            else
            {
                carModel = new CarModel()
                {
                    Brand = c.Brand,
                    CarID = c.CarID,
                    //Comments = c.Comments,
                    Consuption = c.Consuption,
                    Description = c.Description,
                    Doors = c.Doors,
                    // TODO Location not working 
                    Site = c.Location,
                    NumberPlate = c.NumberPlate,
                    Passangers = c.Passangers,
                    Power = c.Power,
                    Price = c.Price,
                    State = c.State,
                    Trunk = c.Trunk,
                    Type = c.Type
                };
            }

            if(images != null)
            {
                foreach (var img in images)
                {
                    var image = new ImageModel()
                    {
                        Car = carModel,
                        Path = img.Path,
                        Name = img.Name
                    };
                    context.Images.Add(image);
                }
            }
            
            if (context.Cars.Any(car => car.CarID == carModel.CarID))
            {
                context.Cars.Update(carModel);
            }
            else
            {
                context.Cars.Add(carModel);
            }

            site.Cars.Add(carModel);
            context.Sites.Update(site);

            context.SaveChanges();
        }

        public ImageDTO GetImage(int id)
        {
            var img = context.Images
                .Include(i => i.Car)
                .Where(i => i.ImageID == id)
                .Select(i => new ImageDTO()
                {
                    ImageID = i.ImageID,
                    Car = i.Car,
                    Name = i.Name,
                    Path = i.Path
                }).FirstOrDefault();
            
            return img;
        }

        public void DeleteImage(int id)
        {
            var img = context.Images.Find(id);
            context.Images.Remove(img);
            context.SaveChanges();
        }

        // Manage Rents
        public List<RentDTO> GetRents()
        {
            var rents = context.Rents
                .Include(r => r.Car)
                .Where(r => r.Finished)
                .Select(r => new RentDTO()
                {
                    RentID = r.RentID,
                    Car = r.Car,
                    RentStarts = r.RentStarts,
                    RentEnds = r.RentEnds,
                    State = r.State,
                    Finished = r.Finished
                }).ToList();
            return rents;
        }

        public List<RentDTO> GetRentsForUser(ApplicationUser user)
        {
            var rents = context.Rents
                .Include(r => r.Car)
                .Where(r => r.Finished && r.User.Id == user.Id)
                .Select(r => new RentDTO()
                {
                    RentID = r.RentID,
                    Car = r.Car,
                    RentStarts = r.RentStarts,
                    RentEnds = r.RentEnds,
                    State = r.State,
                    Finished = r.Finished
                }).ToList();
            return rents;
        }

        public List<RentDetailsDTO> GetRentsDetailed()
        {
            var rents = context.Rents
                .Include( r => r.Car )
                .Include( r => r.Site )
                .Include( r => r.User )
                .Where( r => r.Finished)
                .Select( r => new RentDetailsDTO()
                {
                    RentID = r.RentID,
                    Car = r.Car,
                    Site = r.Site,
                    User = r.User,
                    RentStarts = r.RentStarts,
                    RentEnds = r.RentEnds,
                    Price = r.Price,
                    Insurance = r.Insurance,
                    State = r.State, 
                    Finished = r.Finished
                }).ToList();
            return rents;
        }

        public RentDetailsDTO GetRent(int id)
        {
            var rent = context.Rents
                //.Where(r => r.RentID == id)
                .Include(r => r.Car)
                .Include(r => r.Site)
                .Include(r => r.User)
                .Single(r => r.RentID == id);

            var _rent = new RentDetailsDTO()
            {
                RentID = rent.RentID,
                Car = rent.Car,
                Site = rent.Site,
                User = rent.User,
                RentStarts = rent.RentStarts,
                RentEnds = rent.RentEnds,
                Price = rent.Price,
                Insurance = rent.Insurance,
                State = rent.State,
                Finished = rent.Finished
            };

            return _rent;
        }

        public int CreateRent(RentDetailsDTO _rent)
        {
            var user = context.Users.Single(u => u.Id == _rent.User.Id);
            var car = context.Cars.Single(c => c.CarID == _rent.Car.CarID);
            var site = context.Sites.Single(s => s.SiteID == _rent.Site.SiteID);
            var rent = new CarRentModels.RentModel()
            {
                //RentID = _rent.RentID,
                Car = car,
                Site = site,
                User = user,
                RentStarts = _rent.RentStarts,
                RentEnds = _rent.RentEnds,
                Price = _rent.Price,
                Insurance = EnumTypes.InsuranceType.Basic,
                //Insurance = _rent.Insurance,
                State = EnumTypes.RentState.Pending,
                Finished = false
            };
            
            context.Rents.Add(rent);

            context.SaveChanges();

            return rent.RentID;
        }

        public void FinishRent(int id)
        {
            var rent = context.Rents.Single(r => r.RentID == id);

            rent.Finished = true;

            context.Rents.Update(rent);
            context.SaveChanges();
        }

        public void DeleteRent(int id)
        {
            // Csak a kölcsönzést töröljük, a user, autó, telephely sértetlen kell hogy maradjon
            var rent = context.Rents.Find(id);

            context.Rents.Remove(rent);
            context.SaveChanges();
        }

        public void PendingRent(int id)
        {
            var rent = context.Rents.Find(id);
            rent.State = EnumTypes.RentState.Pending;

            context.Rents.Update(rent);
            context.SaveChanges();
        }

        public void ApproveRent(int id)
        {
            var rent = context.Rents.Find(id);
            rent.State = EnumTypes.RentState.Approved;

            context.Rents.Update(rent);
            context.SaveChanges();
        }

        public void DismissRent(int id)
        {
            var rent = context.Rents.Find(id);
            rent.State = EnumTypes.RentState.Dismissed;

            context.Rents.Update(rent);
            context.SaveChanges();
        }

        // Manage Sites
        public List<SiteDTO> GetSites()
        {
            var sites = context.Sites
                .Include( s => s.Cars)
                .Select(s => new SiteDTO()
                {
                    SiteID = s.SiteID,
                    Name = s.Name,
                    Address = s.Address,
                    Cars = s.Cars
                }).ToList();
            return sites;
        }

        public SiteDTO GetSite(int id)
        {
            var sites = context.Sites
                .Where( s => s.SiteID == id )
                .Include( s => s.Cars )
                .Select( s => new SiteDTO()
                {
                    SiteID = s.SiteID,
                    Name = s.Name,
                    Address = s.Address,
                    Cars = s.Cars
                }).First();
            return sites;
        }

        public SiteModel GetSiteByID(int id)
        {
            return context.Sites.Find(id);
        }

        public void DeleteSite(int id)
        {
            // Ilyenkor az oda tartozó autókat ne törölje, csak állítsa null-ra a Site-ját

            var site = context.Sites.Find(id);

            foreach(var car in site.Cars)
            {
                // Lehet elég lenne ez is:
                // car.Site = null;
                var c = context.Cars.Find(car);
                c.Site = null;
            }

            context.Sites.Remove(site);
            context.SaveChanges();
        }

        public List<String> GetCarTypes()
        {
            return Enum.GetNames(typeof(EnumTypes.CarType)).ToList();
        }

        public List<String> GetInsurence()
        {
            return Enum.GetNames(typeof(EnumTypes.InsuranceType)).ToList();
        }
    }
}
