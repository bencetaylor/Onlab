using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.DAL.Data;
using CarRent.DAL.Models;
using CarRent.Models.DataViewModels;
using CarRent.DAL.Models.DTOs;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        private readonly DataController data;
        private CarViewModel model;

        // TODO context only in DAL project
        public CarController()
        {
            data = new DataController();
        }

        // GET: Car
        public ActionResult Index()
        {
            model = new CarViewModel();
            var cars = data.GetCars();
            foreach(var c in  cars)
            {
                model.Cars.Add(c);
            }
            
            return View("../Admin/Cars",model);
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            model = new CarViewModel();
            var car = data.GetCar(id);

            if (car == null)
                return NotFound();
            model.CarFullDetail = car;
            return View("../Admin/CarDetail", model);
        }

        // GET: Car/Create
        public ActionResult Create([Bind("Brand,Type,Location,NumberPlate,Power,Doors,Price,Consuption,Description,Images")] CarDetailsDTO car)
        {
            var model = new CarViewModel();
            if (ModelState.IsValid)
            {
                data.CreateCar(car);
                return RedirectToAction("Index");
            }
            model.CarFullDetail = car;
            return View("../Admin/CarDetail", model);
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            model = new CarViewModel();
            var car = data.GetCar(id);

            if (car == null)
                return NotFound();
            model.CarFullDetail = car;
            return View("../Admin/CarEdit", model);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Car/Delete/5
        public ActionResult Delete(int id)
        {
            model = new CarViewModel();
            var car = data.GetCar(id);

            if (car == null)
                return NotFound();
            model.CarFullDetail = car;
            return View("../Admin/CarDelete", model);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            data.DeleteCar(id);
            return RedirectToAction("Index");
        }
    }
}