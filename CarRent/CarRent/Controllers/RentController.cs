﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models.DataViewModels;
using CarRent.Models;
using CarRent.DAL.Models;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace CarRent.Controllers
{
    public class RentController : Controller
    {
        private readonly DataController data;
        private readonly UserManager<ApplicationUser> userManager;
        private RentViewModel model;

        public RentController(UserManager<ApplicationUser> _userManager)
        {
            data = new DataController();
            userManager = _userManager;
        }

        // GET: Rent
        public ActionResult Index()
        {
            model = new RentViewModel();
            model.Rents = data.GetRents();
            return View("../Admin/Rents", model);
        }

        // GET: Rent/Details/5
        public ActionResult Details(int id)
        {
            
            if (User.IsInRole("ADMIN"))
            {
                model = new RentViewModel();
                var rent = data.GetRent(id);

                if (rent == null)
                    return NotFound();
                model.RentDetails = rent;
                return View("../Admin/RentDetails", model);
            }
            else
            {
                // TODO - user rent detail view
                return View("Home/Index");
            }
        }

        // GET: Rent/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("../Account/Login");
            }
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            

            model = new RentViewModel();

            return View("../Admin/RentCreate", model);
        }

        // POST: Rent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RentViewModel model)
        {
            if (User.IsInRole("ADMIN"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var rent = new RentDetailsDTO();

                        rent.Car = data.GetCar(model.RentDetails.Car.NumberPlate);

                        data.CreateRent(model.RentDetails);
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            else if (User.IsInRole("USER"))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var user = userManager.FindByIdAsync(User.Identity.Name).Result;
                        var rent = new RentViewModel();
                        rent.RentDetails = model.RentDetails;

                        rent.RentDetails.User = user;
                        rent.RentDetails.Car = model.RentDetails.Car;

                        data.CreateRent(model.RentDetails);
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            else
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            
        }

        // GET: Rent/Edit/5
        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            model = new RentViewModel();
            var rent = data.GetRent(id);

            if (rent == null)
                return NotFound();
            model.RentDetails = rent;
            return View("../Admin/RentEdit", model);
        }

        // POST: Rent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    data.CreateRent(model.RentDetails);
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Rent/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            model = new RentViewModel();
            var rent = data.GetRent(id);

            if (rent == null)
                return NotFound();
            model.RentDetails = rent;
            return View("../Admin/RentDelete", model);
        }

        // POST: Rent/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (!User.IsInRole("ADMIN"))
                {
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }
                data.DeleteRent(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}