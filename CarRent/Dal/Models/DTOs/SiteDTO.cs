using CarRent.DAL.Models.CarRentModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.DTOs
{
    class SiteDTO
    {
        public int SiteID { get; set; }
        public String Name { get; set; }
        public String Address { get; set; }
        public ICollection<CarRentModels.CarModel> Cars { get; set; }
    }
}
