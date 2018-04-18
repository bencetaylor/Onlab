using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.DAL.Models;
using CarRent.Models.DataViewModels;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using CarRent.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

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
                model = new CarViewModel();
                var cars = data.GetCars();
                foreach (var c in cars)
                {
                    model.Cars.Add(c);
                }
                return View("../User/Cars", model);
            }
        }

        // GET: Car/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (User.IsInRole("ADMIN"))
            {
                model = new CarViewModel();
                var car = data.GetCar(id);

                if (car == null)
                    return NotFound();
                model.CarFullDetail = car;
                var images = new List<FileStreamResult>();
                foreach(var img in car.Images)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate, img.Path);
                    var memory = new MemoryStream();
                    using(var stream = new FileStream(path, FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    // TODO - file type
                    var file = File(memory,"", Path.GetFileName(path));
                    images.Add(file);
                }
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
        public async Task<ActionResult> Create(CarViewModel model, List<IFormFile> images)
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

                    var imageList = new List<ImageDTO>();

                    foreach (var formFile in images)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate, formFile.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        imageList.Add(new ImageDTO()
                        {
                            Path = formFile.FileName,
                            Name = formFile.FileName
                        });
                    }

                    data.CreateOrUpdateCar(model.CarFullDetail, imageList);
                    
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Index");
            }
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
            model.Sites = data.GetSites();
            var car = data.GetCar(id);

            if (car == null)
                return NotFound();
            model.CarFullDetail = car;
            return View("../Admin/CarEdit", model);
        }

        // POST: Car/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CarViewModel model, List<IFormFile> images)
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
                    model.CarFullDetail.Location = data.GetSiteByID(1);

                    var imageList = new List<ImageDTO>();

                    foreach (var formFile in images)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", formFile.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        imageList.Add(new ImageDTO()
                        {
                            Path = formFile.FileName,
                            Name = formFile.Name
                        });
                    }

                    data.CreateOrUpdateCar(model.CarFullDetail, imageList);
                    return RedirectToAction("Index");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
        // Post: Car/Delete/5
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

            foreach (var img in car.Images)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", img.Path);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            data.DeleteCar(id);
            return RedirectToAction("Index");
        }
    }
}