using CarRent.DAL.Data;
using CarRent.DAL.Models.CarRentModels;
using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRent.DAL.Models.Controllers
{
    class RentController
    {
        private readonly ApplicationDbContext context;

        public RentController(ApplicationDbContext _context)
        {
            context = _context;
        }

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
                          Price =r.Price,
                          RentEnds = r.RentEnds,
                          RentStart = r.RentStart,
                          Site = r.Site,
                          SiteID = r.SiteID,
                          State = r.State
                      };
            return rent;
        }
    }
}
