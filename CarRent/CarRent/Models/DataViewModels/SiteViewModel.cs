using CarRent.DAL.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.DataViewModels
{
    public class SiteViewModel
    {
        public List<SiteDTO> Sites;
        public SiteDTO SiteDetails;

        SiteViewModel()
        {
            Sites = new List<SiteDTO>();
            SiteDetails = new SiteDTO();
        }

    }
}
