using CarRent.DAL.Models.CarRentModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.DTOs
{
    public class ImageDTO
    {
        public int ImageID { get; set; }
        public CarModel Car { get; set; }

        public String Name { get; set; }
        public String Path { get; set; }

        public ImageDTO()
        {
            Car = null;
        }
    }
}
