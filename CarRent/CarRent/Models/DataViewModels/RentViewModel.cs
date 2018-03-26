using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.DataViewModels
{
    public class RentViewModel
    {
        public List<RentDTO> Rents;
        public RentDetailsDTO RentDetails;

        public RentViewModel()
        {
            Rents = new List<RentDTO>();
            RentDetails = new RentDetailsDTO();
        }
    }
}
