using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.DAL.Models;
using CarRent.Models.DataViewModels;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using CarRent.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        private readonly DataController data;
        private CarViewModel model;

        // TODO context only in DAL project
        public CarController(UserManager<ApplicationUser> _userManager)
        {
            data = new DataController();
        }

        // GET: Car
        public ActionResult Index()
        {
            // Admin views
            if(User.IsInRole("ADMIN")){
                model = new CarViewModel();
                var cars = data.GetCars();
                foreach (var c in cars)
                {
                    model.Cars.Add(c);
                }

                return View("../Admin/Cars", model);
            }
            else
            {
                // TODO - user cars view
                return View("../Home/Index");
            }
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            if (User.IsInRole("ADMIN"))
            {
                model = new CarViewModel();
                var car = data.GetCar(id);

                if (car == null)
                    return NotFound();
                model.CarFullDetail = car;
                return View("../Admin/CarDetail", model);
            }
            else
            {
                // TODO - user car detail view
                return View("Home/Index");
            }
        }

        // GET: Car/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            
            model = new CarViewModel();
            model.Types = data.GetCarTypes();
            model.Sites = data.GetSites();

            return View("../Admin/CarCreate", model);
        }

        // POST: Car/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CarViewModel model)
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
                    //if (Request.Form.Files.Count > 0)
                    //{
                    //    var file = Request.Form.Files[0];

                    //    if (file != null && file.Length > 0)
                    //    {
                    //        // TODO resize and save to database
                    //        var fileName = Path.GetFileName(file.FileName);
                    //        var path = Path.Combine("", fileName);

                    //        using (var stream = new FileStream(filePath, FileMode.Create))
                    //        {
                    //            file.CopyTo(stream);
                    //            //await formFile.CopyToAsync(stream);
                    //        }
                    //    }
                    //}

                    //var site = data.GetSite(model.CarFullDetail.Location.Name);
                    //model.CarFullDetail.Location = site;

                    data.CreateOrUpdateCar(model.CarFullDetail);
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // TODO - file feltöltés és mentés adatbázisba??
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> PostFiles(List<IFormFile> files)
        {
            //long size = files.Sum(f => f.Length);

            // könyvtár ahova lementjük a file-t
            // csak a file név és egyéb adatok a db-be
            // wwwroot
            // full path to file in temp location
            
            foreach (var formFile in files)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", formFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            using (var memoryStream = new MemoryStream())
            {
                foreach( var file in files)
                {
                    ImageDTO image = new ImageDTO()
                    {
                        Path = Path.Combine(Directory.GetCurrentDirectory(),
                           "wwwroot", file.FileName),
                        Name = file.Name,
                    };
                    data.SaveImage(image);
                }
            }

            return View();
        }

        // GET: Car/Edit/5
        public ActionResult Edit(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }

            model = new CarViewModel();
            model.Types = data.GetCarTypes();
            var car = data.GetCar(id);

            if (car == null)
                return NotFound();
            model.CarFullDetail = car;
            return View("../Admin/CarEdit", model);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CarViewModel model)
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
                    data.CreateOrUpdateCar(model.CarFullDetail);
                    return RedirectToAction("Index");
                }
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
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
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
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            
            data.DeleteCar(id);
            return RedirectToAction("Index");
        }

        
    }
}