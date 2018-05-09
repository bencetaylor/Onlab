using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.DAL.Models.CarRentModels
{
    public class SiteModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SiteID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public float Lon { get; set; }
        public float Lat { get; set; }
        public ICollection<CarModel> Cars { get; set; }

        public SiteModel()
        {
            Cars = new List<CarModel>();
        }
    }
}
