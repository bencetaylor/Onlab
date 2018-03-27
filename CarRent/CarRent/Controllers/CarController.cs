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

        //[HttpPost]
        //public ActionResult UploadImageMethod()
        //{
        //    if (Request.Form.Files.Count != 0)
        //    {
        //        for (int i = 0; i < Request.Form.Files.Count; i++)
        //        {
        //            System.Web.

        //            HttpUtility file = Request.Form.Files[i];
        //            int fileSize = file.ContentLength;
        //            string fileName = file.FileName;
        //            file.SaveAs(Server.MapPath("~/Upload_Files/" + fileName));
        //            ImageGallery imageGallery = new ImageGallery();
        //            imageGallery.ID = Guid.NewGuid();
        //            imageGallery.Name = fileName;
        //            imageGallery.ImagePath = "~/Upload_Files/" + fileName;
        //            db.ImageGallery.Add(imageGallery);
        //            db.SaveChanges();
        //        }
        //        return Content("Success");
        //    }
        //    return Content("failed");
        //}
        

        // TODO - file feltöltés és mentés adatbázisba??
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> PostFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            using (var memoryStream = new MemoryStream())
            {
                //await model.AvatarImage.CopyToAsync(memoryStream);
                //user.AvatarImage = memoryStream.ToArray();
                ImageDTO image = new ImageDTO()
                {
                     Content = memoryStream.ToArray(),
                     Name = files.First().Name,
                    
                };
            }

            return Ok(new { count = files.Count, size, filePath });
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