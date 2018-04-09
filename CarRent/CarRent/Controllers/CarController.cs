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
        public ActionResult Create(CarViewModel model, List<IFormFile> images)
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
                    //var site = data.GetSite(model.CarFullDetail.Location.Name);
                    //model.CarFullDetail.Location = site;

                    // Valamiért a site null marad és nem lehet hozzáadni az autóhoz...
                    //var site = model.CarFullDetail.Location;


                    // Addig minden autó a Site1-be kerül
                    model.CarFullDetail.Location = data.GetSiteByID(1);

                    data.CreateOrUpdateCar(model.CarFullDetail);

                    var imageList = new List<ImageDTO>();

                    // Képek feltöltése
                    foreach (var formFile in images)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", formFile.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyToAsync(stream);
                        }
                        var image = new ImageDTO()
                        {
                            Car = data.FindCar(model.CarFullDetail.CarID),
                            Path = filePath,
                            Name = formFile.Name
                        };
                        data.SaveImage(image);
                    }
                    
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        
        // TODO - file feltöltés és mentés adatbázisba??
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> PostFiles(List<IFormFile> files)
        {
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
                var image = new ImageDTO()
                {
                    Path = filePath,
                    Name = formFile.Name
                };
                data.SaveImage(image);
            }


            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //using (var memoryStream = new MemoryStream())
            //{
            //    foreach( var file in files)
            //    {
            //        ImageDTO image = new ImageDTO()
            //        {
            //            Path = Path.Combine(Directory.GetCurrentDirectory(),
            //               "wwwroot", file.FileName),
            //            Name = file.Name,
            //        };
            //        data.SaveImage(image);
            //    }
            //}

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

        /*
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
        */
        /*
            // POST: Car/Delete/5
            [HttpPost]
            public ActionResult Delete(int id)
            {
                if (!User.IsInRole("ADMIN"))
                {
                    var errorModel = new ErrorViewModel();
                    return View("../Shared/Error", errorModel);
                }

                data.DeleteCar(id);
                return RedirectToAction("Index");
            }
            */


        // GET: Car/Delete/5
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            var car = data.GetCar(id);
            if (car == null)
                return NotFound();

            data.DeleteCar(id);
            return RedirectToAction("Index");
        }
    }
}