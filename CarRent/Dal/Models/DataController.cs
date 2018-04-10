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
                    //Image = c.Images.First()
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
                    //Image = c.Images.First()
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
                }).First();
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

            //if()
            //{
            //    carModel = 
            //}

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

        // Manage Rents
        public List<RentDTO> GetRents()
        {
            var rents = context.Rents
                .Include(r => r.Car)
                .Select(r => new RentDTO()
                {
                    RentID = r.RentID,
                    Car = r.Car,
                    RentStarts = r.RentStarts,
                    RentEnds = r.RentEnds
                }).ToList();
            return rents;
        }

        public List<RentDetailsDTO> GetRentsDetailed()
        {
            var rents = context.Rents
                .Include( r => r.Car )
                .Include( r => r.Site )
                .Include( r => r.User )
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
                    State = r.State
                    
                }).ToList();
            return rents;
        }

        public RentDetailsDTO GetRent(int id)
        {
            var rent = context.Rents
                .Where( r => r.RentID == id )
                .Include( r => r.Car )
                .Include( r => r.Site )
                .Include( r => r.User )
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
                    State = r.State

                }).First();

            return rent;
        }

        public Boolean CreateRent(RentDetailsDTO _rent)
        {
            var rent = new CarRentModels.RentModel()
            {
                RentID = _rent.RentID,
                Car = _rent.Car,
                Site = _rent.Site,
                User = _rent.User,
                RentStarts = _rent.RentStarts,
                RentEnds = _rent.RentEnds,
                Price = _rent.Price,
                Insurance = _rent.Insurance,
                State = _rent.State
            };
            context.Rents.Add(rent);
            context.SaveChanges();
            // TODO check that the save was successful
            return true;
        }

        public void DeleteRent(int id)
        {
            // Csak a kölcsönzést töröljük, a user, autó, telephely sértetlen kell hogy maradjon
            var rent = context.Rents.Find(id);

            context.Rents.Remove(rent);
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

        public void SaveImages(List<ImageDTO> images, int CarID)
        {
            var car = context.Cars.Find(CarID);
            foreach (var img in images)
            {
                var image = new ImageModel()
                {
                    Car = img.Car,
                    Name = img.Name,
                    Path = img.Path
                };
                context.Images.Add(image);
                car.Images.Add(image);
            }
            context.Cars.Update(car);
            context.SaveChanges();
        }

        public List<String> GetCarTypes()
        {
            return Enum.GetNames(typeof(CarTypes.Types)).ToList();
        }
    }
}
