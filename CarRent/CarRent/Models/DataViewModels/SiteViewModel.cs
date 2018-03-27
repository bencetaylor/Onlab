using CarRent.DAL.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.DataViewModels
{
    public class SiteViewModel
    {
        public List<SiteDTO> Sites;
        [BindProperty]
        public SiteDTO SiteDetails { get; set; }

        public SiteViewModel()
        {
            Sites = new List<SiteDTO>();
            SiteDetails = new SiteDTO();
        }

    }
}
