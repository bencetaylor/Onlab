using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.DataViewModels
{
    public class RentViewModel
    {
        public List<RentDTO> Rents;
        [BindProperty]
        public RentDetailsDTO RentDetails { get; set; }

        public RentViewModel()
        {
            Rents = new List<RentDTO>();
            RentDetails = new RentDetailsDTO();
        }
    }
}
