using System;
using System.Collections.Generic;
using System.Text;
using CarRent.DAL.Models.CarRentModels;
using System.Linq;

namespace CarRent.DAL.Models.DTOs
{
    public class CarDTO
    {
        public int CarID { get; set; }
        public int Price { get; set; }
        public String Type { get; set; }
        public String Brand { get; set; }
        public String Location { get; set; }
        public String NumberPlate { get; set; }

        public ImageModel Image { get; set; }
    }
}
