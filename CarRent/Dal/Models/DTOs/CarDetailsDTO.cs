using CarRent.DAL.Models.CarRentModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.DTOs
{
    class CarDetailsDTO
    {
        public int CarID { get; set; }
        public String NumberPlate { get; set; }
        public String Type { get; set; }
        public String Brand { get; set; }
        public int Price { get; set; }
        public int Doors { get; set; }
        public int Passangers { get; set; }
        public int Consuption { get; set; }
        public int Trunk { get; set; }
        public int Power { get; set; }
        public String Description { get; set; }

        public CarState State { get; set; }

        public SiteModel Location { get; set; }
        public ICollection<ImageModel> Images { get; set; }
        public ICollection<CommentModel> Comments { get; set; }
    }
}
