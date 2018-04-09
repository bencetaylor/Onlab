using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.Models.DataViewModels;
using CarRent.DAL.Models;
using CarRent.Models;

namespace CarRent.Controllers
{
    public class SiteController : Controller
    {
        private readonly DataController data;
        private SiteViewModel model;

        public SiteController()
        {
            data = new DataController();
        }

        // GET: Site
        public ActionResult Index()
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            model = new SiteViewModel();
            var sites = data.GetSites();
            foreach (var c in sites)
            {
                model.Sites.Add(c);
            }

            return View("../Admin/Sites", model);
        }

        // GET: Site/Details/5
        public ActionResult Details(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            var model = new SiteViewModel();
            model.SiteDetails = data.GetSite(id);
            model.carCount = model.SiteDetails.Cars.Count();

            return View("../Admin/SiteDetail", model);
        }

        // GET: Site/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            return View();
        }

        // POST: Site/Create
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

        // GET: Site/Edit/5
        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            var model = new SiteViewModel();
            model.SiteDetails = data.GetSite(id);

            return View("../Admin/SiteEdit", model);
        }

        // POST: Site/Edit/5
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

        // GET: Site/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            return View();
        }

        // POST: Site/Delete/5
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