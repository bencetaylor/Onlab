using CarRent.DAL.Data;
using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRent.DAL.Models
{
    class DataController
    {
        private readonly ApplicationDbContext context;

        public DataController(ApplicationDbContext _context)
        {
            context = _context;
        }

        // Manage Cars
        public IQueryable<CarDTO> GetCars()
        {
            var cars = from c in context.Cars
                       select new CarDTO()
                       {
                           ID = c.CarID,
                           Price = c.Price,
                           Type = c.Type,
                           Brand = c.Brand,
                           Image = c.Images.First()
                       };
            return cars;
        }

        public IQueryable<CarDetailsDTO> GetCarsDetailed()
        {
            var cars = from c in context.Cars
                       select new CarDetailsDTO()
                       {
                           Brand = c.Brand,
                           CarID = c.CarID,
                           Consuption = c.Consuption,
                           Description = c.Description,
                           Doors = c.Doors,
                           Location = c.Location,
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
                           Location = c.Location,
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

        public IQueryable<CarDetailsDTO> GetCar(int id)
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
                          Images = c.Images,
                          Location = c.Location,
                          NumberPlate = c.NumberPlate,
                          Passangers = c.Passangers,
                          Power = c.Power,
                          Price = c.Price,
                          State = c.State,
                          Trunk = c.Trunk,
                          Type = c.Type
                      };
            return car;
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
                           CarID = r.CarID,
                           Car = r.Car,
                           Insurance = r.Insurance,
                           User = r.User,
                           Price = r.Price,
                           RentEnds = r.RentEnds,
                           RentStart = r.RentStart,
                           Site = r.Site,
                           SiteID = r.SiteID,
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
                           CarID = r.CarID,
                           Car = r.Car,
                           Insurance = r.Insurance,
                           User = r.User,
                           Price = r.Price,
                           RentEnds = r.RentEnds,
                           RentStart = r.RentStart,
                           Site = r.Site,
                           SiteID = r.SiteID,
                           State = r.State
                       };
            return rent;
        }

        // Manage Sites
        public IQueryable<SiteDTO> GetSites()
        {
            var sites = from s in context.Sites
                        select new SiteDTO()
                        {
                            SiteID = s.SiteID,
                            Address = s.Address,
                            Name = s.Name
                        };
            return sites;
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
    }
}
