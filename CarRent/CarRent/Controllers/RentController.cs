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
using System.Text;

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
            if (User.IsInRole("ADMIN"))
            {
                model = new RentViewModel();
                model.Rents = data.GetRents();
                return View("../Admin/Rents", model);
            }
            else
            {
                model = new RentViewModel();
                var user = userManager.FindByNameAsync(User.Identity.Name).Result;
                model.Rents = data.GetRentsForUser(user);
                return View("../User/Rents", model);
            }
            
        }

        // GET: Rent/Details/5
        public ActionResult Details(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("../Account/Login");
            }

            model = new RentViewModel();
            var rent = data.GetRent(id);

            if (rent == null)
                return NotFound();
            model.RentDetails = rent;

            if (User.IsInRole("ADMIN"))
            {
                return View("../Admin/RentDetail", model);
            }
            else
            {
                return View("../User/RentDetail", model);
            }
        }

        // GET: Rent/Create
        public ActionResult Create(int carID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("../Account/Login");
            }
            //if (!User.IsInRole("ADMIN"))
            //{
            //    var errorModel = new ErrorViewModel();
            //    return View("../Shared/Error", errorModel);
            //}
            model = new RentViewModel();
            model.Insurance = data.GetInsurence();
            model.RentDetails.Car = data.FindCar(carID);
            
            return View("../User/RentCreate", model);
        }
        // POST: Rent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RentViewModel model, int CarID)
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var user = userManager.FindByNameAsync(User.Identity.Name).Result;

                        model.RentDetails.User = user;

                        var car = data.FindCar(model.RentDetails.Car.CarID);
                        model.RentDetails.Car = car;

                        model.RentDetails.Price = Convert.ToInt32((model.RentDetails.RentEnds - model.RentDetails.RentStarts).TotalDays) * car.Price;
                        model.RentDetails.Site = data.GetSiteByID(1);

                        int RentID = data.CreateRent(model.RentDetails);

                        return PreviewRent(RentID);
                    }
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }
                catch (Exception e)
                {
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }
            }
            return View();

        }

        public ActionResult PreviewRent(int RentID)
        {
            model = new RentViewModel();
            model.RentDetails = data.GetRent(RentID);

            return View("../User/RentPreview", model);
        }
        
        public ActionResult FinishRent(int id)
        {
            // Ide valamiért 0 jön a rendes ID helyett
            //RentID = 1022;
            data.FinishRent(id);

            return RedirectToAction("Index");
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
            model.Insurance = data.GetInsurence();

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
                return RedirectToAction("../Home/Index");
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
        
        // POST: Rent/Pending/5
        public ActionResult Pending(int id)
        {
            try
            {
                if (!User.IsInRole("ADMIN"))
                {
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }
                var rent = data.GetRent(id);
                if (rent == null)
                    return NotFound();

                data.PendingRent(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Rent/Approved/5
        public ActionResult Approve(int id)
        {
            try
            {
                if (!User.IsInRole("ADMIN"))
                {
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }
                var rent = data.GetRent(id);
                if (rent == null)
                    return NotFound();

                data.ApproveRent(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Rent/Dismissed/5
        public ActionResult Dismiss(int id)
        {
            try
            {
                if (!User.IsInRole("ADMIN"))
                {
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }
                var rent = data.GetRent(id);
                if (rent == null)
                    return NotFound();

                data.DismissRent(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}