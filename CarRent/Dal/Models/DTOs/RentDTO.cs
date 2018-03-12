using CarRent.DAL.Models.CarRentModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.DTOs
{
    public class RentDTO
    {
        public int ID { get; set; }
        public CarModel Car { get; set; }
        public DateTime RentsStart { get; set; }
        public DateTime RentEnds { get; set; }
    }
}
