using CarRent.DAL.Data;
using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarRent.DAL.Models.Controllers
{
    class SiteController
    {
        private readonly ApplicationDbContext context;

        public SiteController(ApplicationDbContext _context)
        {
            context = _context;
        }

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
