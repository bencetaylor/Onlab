using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.DAL.Models.CarRentModels
{
    public class CarModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }
        public SiteModel Site { get; set; }

        public ICollection<ImageModel> Images { get; set; }
        public ICollection<CommentModel> Comments { get; set; }

        public String NumberPlate { get; set; }
        public EnumTypes.CarType Type { get; set; }
        public String Brand { get; set; }
        public int Price { get; set; }
        public int Doors { get; set; }
        public int Passangers { get; set; }
        public int Consuption { get; set; }
        public int Trunk { get; set; }
        public int Power { get; set; }
        public String Description { get; set; }
        public EnumTypes.CarState State { get; set; }

        public CarModel()
        {
            Site = null;
            Images = new List<ImageModel>();
            Comments = new List<CommentModel>();
        }
    }
}
