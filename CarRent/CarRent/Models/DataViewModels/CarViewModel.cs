using System.Collections.Generic;
using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace CarRent.Models.DataViewModels
{
    public class CarViewModel
    {
        public List<CarDTO> Cars;
        public List<CarDetailsDTO> CarsDetail;
        [BindProperty]
        public CarDetailsDTO CarFullDetail { get; set; }

        public CarViewModel()
        {
            Cars = new List<CarDTO>();
            CarsDetail = new List<CarDetailsDTO>();
            CarFullDetail = new CarDetailsDTO();
        }
    }
}
