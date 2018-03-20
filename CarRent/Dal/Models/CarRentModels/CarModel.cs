﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.DAL.Models.CarRentModels
{
    public enum CarState { Rented = 1, Available=2, InService=3 };

    public class CarModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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

        public CarModel()
        {
            Location = null;
            Images = null;
            Comments = null;
        }
    }
}
