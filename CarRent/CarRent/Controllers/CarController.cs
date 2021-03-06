﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRent.DAL.Models;
using CarRent.Models.DataViewModels;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using CarRent.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System;

namespace CarRent.Controllers
{
    public class CarController : Controller
    {
        private readonly DataController data;
        private CarViewModel model;

        public CarController()
        {
            data = new DataController();
        }

        // GET: Car
        public ActionResult Index(string MinPrice, string MaxPrice, string MinPassanger, string MaxPassanger, CarViewModel model)
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

                //if(model == null)
                //    model = new CarViewModel();
                //var siteID = model.SearchViewModel.Site;

                //if(siteID == 0)
                //{
                //    siteID = 0;
                //}

                var cars = data.SearchCars(InitSearchValues(MinPrice, MaxPrice, MinPassanger, MaxPassanger), 0);
                //var cars = data.SearchCars(InitSearchValues(MinPrice, MaxPrice, MinPassanger, MaxPassanger), siteID);

                foreach (var c in cars)
                {
                    if (c.Image == null)
                    {
                        c.Image = new DAL.Models.CarRentModels.ImageModel()
                        {
                            Path = "http://via.placeholder.com/150x100"
                        };
                    }
                    model.Cars.Add(c);
                }

                var sites = data.GetSites();
                model.Sites = sites;

                return View("../User/Cars", model);
            }
        }

        // GET: Car/Details/5
        public ActionResult Details(int id)
        {
            // Elvileg nem lehet admin hívás
            if (User.IsInRole("ADMIN"))
            {
                model = new CarViewModel();
                var car = data.GetCar(id);

                if (car == null)
                    return NotFound();
                model.CarFullDetail = car;
                //var images = new List<FileStreamResult>();
                //foreach(var img in car.Images)
                //{
                //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate, img.Path);
                //    var memory = new MemoryStream();
                //    using(var stream = new FileStream(path, FileMode.Open))
                //    {
                //        await stream.CopyToAsync(memory);
                //    }
                //    // TODO - file type
                //    var file = File(memory,"", Path.GetFileName(path));
                //    images.Add(file);
                //}
                return View("../Admin/CarDetail", model);
            }
            else
            {
                model = new CarViewModel();
                var car = data.GetCar(id);

                if (car == null)
                    return NotFound();
                model.CarFullDetail = car;

                return View("../User/CarDetail", model);
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
                        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate, formFile.FileName);

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

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
                        var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.CarFullDetail.NumberPlate, formFile.FileName);

                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", car.NumberPlate, img.Path);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", car.NumberPlate);
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath);
            }

            data.DeleteCar(id);
            return RedirectToAction("Index");
        }

        // Post: DeleteImage/5
        public ActionResult DeleteImage(int id)
        {
            if (!User.IsInRole("ADMIN"))
            {
                var errorModel = new ErrorViewModel();
                return View("../Shared/Error", errorModel);
            }
            var img = data.GetImage(id);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", img.Car.NumberPlate, img.Path);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            data.DeleteImage(id);

            //return Redirect(DeleteImage.UrlReferrer.ToString());
            return RedirectToAction("Index");
        }

        public int[] InitSearchValues(string MinPrice, string MaxPrice, string MinPassanger, string MaxPassanger)
        {
            Int32[] res = new Int32[4];
            if (String.IsNullOrEmpty(MinPrice))
            {
                res[0] = 0;
            }
            else
            {
                res[0] = Int32.Parse(MinPrice);
            }
            if (String.IsNullOrEmpty(MaxPrice))
            {
                res[1] = 1000000;
            }
            else
            {
                res[1] = Int32.Parse(MaxPrice);
            }
            if (String.IsNullOrEmpty(MinPassanger))
            {
                res[2] = 0;
            }
            else
            {
                res[2] = Int32.Parse(MinPassanger);
            }
            if (String.IsNullOrEmpty(MaxPassanger))
            {
                res[3] = 20;
            }
            else
            {
                res[3] = Int32.Parse(MaxPassanger);
            }

            return res;
        }
    }
}