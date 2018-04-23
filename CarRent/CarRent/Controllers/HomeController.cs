using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models;
using CarRent.DAL.Models;
using Microsoft.AspNetCore.Identity;
using CarRent.Models.HomeViewModels;
using CarRent.Models.DataViewModels;

namespace CarRent.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataController data;
        private HomeViewModel model;

        public HomeController()
        {
            data = new DataController();
        }

        public IActionResult Index()
        {
            model = new HomeViewModel();
            var cars = data.GetTopCars();
            model.mainCar = cars.First();
            if(model.mainCar.Image == null)
            {
                model.mainCar.Image = new DAL.Models.CarRentModels.ImageModel()
                {
                    Path = "http://via.placeholder.com/800x530"
                };
            }

            cars.RemoveAt(0);

            foreach (var c in cars)
            {
                if (c.Image == null)
                {
                    c.Image = new DAL.Models.CarRentModels.ImageModel()
                    {
                        Path = "http://via.placeholder.com/150x100"
                    };
                }
                model.cars.Add(c);
            }

            return View(model);
        }

        public IActionResult CarDetails(int id)
        {
            var model = new CarViewModel();
            model.CarFullDetail = data.GetCar(id);

            return View("../Home/CarDetails", model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Cars()
        {
            ViewData["Message"] = "Admin view cars page.";
            return View("../Admin/Cars");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
