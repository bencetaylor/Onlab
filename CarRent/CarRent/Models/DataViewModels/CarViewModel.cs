using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRent.DAL.Data;
using CarRent.DAL.Models.CarRentModels;

namespace CarRent.Models.DataViewModels
{
    public class CarViewModel
    {
        public IList<CarModel> Cars;
    }
}
