﻿using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public List<CarDTO> cars;
        public CarDTO mainCar;

        public HomeViewModel()
        {
            cars = new List<CarDTO>();
            mainCar = new CarDTO();
        }
    }
}
