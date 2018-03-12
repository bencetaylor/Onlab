﻿using CarRent.DAL.Models.CarRentModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.DTOs
{
    class RentDetailsDTO
    {
        public int RentID { get; set; }
        public int CarID { get; set; }
        public int SiteID { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentEnds { get; set; }
        public int Price { get; set; }

        public ApplicationUser User { get; set; }
        public CarModel Car { get; set; }
        public SiteModel Site { get; set; }

        public RentState State { get; set; }
        public InsuranceType Insurance { get; set; }
    }
}