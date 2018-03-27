using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models.DataViewModels;
using CarRent.Models;
using CarRent.DAL.Models;

namespace CarRent.Controllers
{
    public class RentController : Controller
    {
        private readonly DataController data;
        private RentViewModel model;

        public RentController()
        {
            data = new DataController();
        }

        // GET: Rent
        public ActionResult Index()
        {
            model = new RentViewModel();
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

            return View();
        }

        // POST: Rent/Create
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

            return View();
        }

        // POST: Rent/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}