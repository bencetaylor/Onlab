﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models.CarRentModels;

namespace CarRent.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Cars.Any())
            {
                return;
            }

            //var cars1 = new CarModel[]
            //{
            //new CarModel{CarID=1, Brand="BMW 316i", NumberPlate="ABC123", Type="Sedan", Price=55000, Consuption=4, Doors=4, Passangers=5, Power=150, Trunk=400, State=CarState.Available},
            //new CarModel{CarID=2, Brand="BMW 330xd", NumberPlate="ABC123", Type="Sedan", Price=80000}
            //};
            //foreach (CarModel c in cars1)
            //{
            //    context.Cars.Add(c);
            //}

            //var cars2 = new CarModel[]
            //{
            //new CarModel{CarID=3, Brand="BMW 320d", NumberPlate="ABC123", Type="Sedan", Price=60000},
            //new CarModel{CarID=4, Brand="BMW 318d touring", NumberPlate="ABC123", Type="Combi", Price=70000},
            //};
            //foreach (CarModel c in cars2)
            //{
            //    context.Cars.Add(c);
            //}


            var sites = new SiteModel[]
            {
            new SiteModel{SiteID=1, Name="Site1", Address="Address1"},
            new SiteModel{SiteID=2, Name="Site2", Address="Address2" },
            };
            foreach (SiteModel s in sites)
            {
                context.Sites.Add(s);
            }

            // A SaveChanges-nél nullPointerException-t dob és nem tölti fel az adatbázist
            context.SaveChanges();

            //var rents = new RentModel[]
            //{
            //    new RentModel{}
            //};

            //foreach (RentModel r in rents)
            //{
            //    context.Rents.Add(r);
            //}
            //context.SaveChanges();
        }
    }
}
