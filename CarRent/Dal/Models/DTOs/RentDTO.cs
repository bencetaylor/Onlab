using CarRent.DAL.Models.CarRentModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.DTOs
{
    public class RentDTO
    {
        public int RentID { get; set; }
        public CarModel Car { get; set; }
        public DateTime RentStarts { get; set; }
        public DateTime RentEnds { get; set; }
        public bool Finished { get; set; }
        public EnumTypes.RentState State { get; set; }
    }
}
