using CarRent.DAL.Data;
using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRent.DAL.Models.Controllers
{
    class CarController
    {
        private readonly ApplicationDbContext context;

        public CarController(ApplicationDbContext _context)
        {
            context = _context;
        }

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
    }
}
