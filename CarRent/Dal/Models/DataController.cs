using CarRent.DAL.Data;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                .Select(c => new CarDTO
                {
                    ID = c.CarID,
                    Price = c.Price,
                    Type = c.Type ?? "none",
                    Brand = c.Brand ?? "none",
                    // TODO - site shows as null???
                    Location = c.Site.Address,
                    Plate = c.NumberPlate,
                    //car.Image = c.Images.First() ?? null;
                }).ToList();
            
            return cars;
        }

        public IList<CarDetailsDTO> GetCarsDetailed()
        {
            var cars = (from c in context.Cars
                       select new CarDetailsDTO
                       {
                           Brand = c.Brand,
                           CarID = c.CarID,
                           Consuption = c.Consuption,
                           Description = c.Description,
                           Doors = c.Doors,
                           Location = c.Site,
                           NumberPlate = c.NumberPlate,
                           Passangers = c.Passangers,
                           Power = c.Power,
                           Price = c.Price,
                           State = c.State,
                           Trunk = c.Trunk,
                           Type = c.Type
                       }).ToList();
            return cars;
        }

        public IQueryable<CarDetailsDTO> GetCarsWithFullDetail()
        {
            var cars = from c in context.Cars
                       select new CarDetailsDTO()
                       {
                           Brand = c.Brand,
                           CarID = c.CarID,
                           Comments = c.Comments,
                           Consuption = c.Consuption,
                           Description = c.Description,
                           Doors = c.Doors,
                           Images = c.Images,
                           Location = c.Site,
                           NumberPlate = c.NumberPlate,
                           Passangers = c.Passangers,
                           Power = c.Power,
                           Price = c.Price,
                           State = c.State,
                           Trunk = c.Trunk,
                           Type = c.Type
                       };
            return cars;
        }

        public CarDetailsDTO GetCar(int id)
        {
            var car = from c in context.Cars
                      where c.CarID == id
                      select new CarDetailsDTO()
                      {
                          Brand = c.Brand,
                          CarID = c.CarID,
                          Comments = c.Comments,
                          Consuption = c.Consuption,
                          Description = c.Description,
                          Doors = c.Doors,
                          //Images = c.Images,
                          // TODO Location not working 
                          //Location = c.Site,
                          NumberPlate = c.NumberPlate,
                          Passangers = c.Passangers,
                          Power = c.Power,
                          Price = c.Price,
                          State = c.State,
                          Trunk = c.Trunk,
                          Type = c.Type
                      };
            return car.First();
        }

        public void DeleteCar(int id)
        {
            var car = context.Cars.Find(id);
            context.Cars.Remove(car);
            context.SaveChanges();
        }

        public void CreateCar(CarDetailsDTO c)
        {
            var carmodel = new CarRentModels.CarModel()
            {
                Brand = c.Brand,
                CarID = c.CarID,
                //Comments = c.Comments,
                Consuption = c.Consuption,
                Description = c.Description,
                Doors = c.Doors,
                //Images = c.Images,
                // TODO Location not working 
                //Location = c.Site,
                NumberPlate = c.NumberPlate,
                Passangers = c.Passangers,
                Power = c.Power,
                Price = c.Price,
                State = c.State,
                Trunk = c.Trunk,
                Type = c.Type
            };
            context.Cars.Add(carmodel);
            context.SaveChanges();
        }

        // Manage Rents
        public IQueryable<RentDTO> GetRents()
        {
            var rents = from r in context.Rents
                        select new RentDTO()
                        {
                            ID = r.RentID,
                            Car = r.Car,
                            RentEnds = r.RentEnds,
                            RentsStart = r.RentStart
                        };
            return rents;
        }

        public IQueryable<RentDetailsDTO> GetRentsDetailed()
        {
            var rent = from r in context.Rents
                       select new RentDetailsDTO()
                       {
                           RentID = r.RentID,
                           //CarID = r.CarID,
                           Car = r.Car,
                           Insurance = r.Insurance,
                           User = r.User,
                           Price = r.Price,
                           RentEnds = r.RentEnds,
                           RentStart = r.RentStart,
                           Site = r.Site,
                           //SiteID = r.SiteID,
                           State = r.State
                       };
            return rent;
        }

        public IQueryable<RentDetailsDTO> GetRent(int id)
        {
            var rent = from r in context.Rents
                       where r.RentID == id
                       select new RentDetailsDTO()
                       {
                           RentID = r.RentID,
                           //CarID = r.CarID,
                           Car = r.Car,
                           Insurance = r.Insurance,
                           User = r.User,
                           Price = r.Price,
                           RentEnds = r.RentEnds,
                           RentStart = r.RentStart,
                           Site = r.Site,
                          // SiteID = r.SiteID,
                           State = r.State
                       };
            return rent;
        }

        // Manage Sites
        public IList<SiteDTO> GetSites()
        {
            var list = context.Sites.ToList();
            var sitelist = new List<SiteDTO>();

            var sites = from s in context.Sites
                        select new SiteDTO()
                        {
                            SiteID = s.SiteID,
                            Address = s.Address,
                            Name = s.Name
                        };
            return sites.ToList();
        }

        public IQueryable<SiteDTO> GetSite(int id)
        {
            var site = from s in context.Sites
                       where s.SiteID == id
                       select new SiteDTO()
                       {
                           SiteID = s.SiteID,
                           Address = s.Address,
                           Name = s.Name,
                           Cars = s.Cars
                       };
            return site;
        }

        public void DeleteSite(int id)
        {
            var site = context.Sites.Find(id);
            context.Sites.Remove(site);
            context.SaveChanges();
        }
    }
}
