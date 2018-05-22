using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.DataViewModels
{
    public class SearchViewModel
    {
        public int MinPrice;
        public int MaxPrice;
        public int MinPassanger;
        public int MaxPassanger;
        public int Site;
    }
}
