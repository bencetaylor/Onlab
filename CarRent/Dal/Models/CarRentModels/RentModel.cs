using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.DAL.Models.CarRentModels
{
    public class RentModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RentID { get; set; }
        public DateTime RentStarts { get; set; }
        public DateTime RentEnds { get; set; }
        public int Price { get; set; }

        public ApplicationUser User { get; set; }
        public CarModel Car { get; set; }
        public SiteModel Site { get; set; }

        public EnumTypes.RentState State { get; set; }
        public EnumTypes.InsuranceType Insurance { get; set; }

        public RentModel()
        {
            User = null;
            Car = null;
            Site = null;
        }
    }
}
