using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRent.Models.CarRentModels
{
    public enum InsuranceType { Basic = 1, Gold=2, Platinum=3 }
    public enum RentState { Approved = 1, Dismissed=2, Pending=3 }

    public class RentModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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

        public RentModel()
        {
            User = new ApplicationUser();
            Car = new CarModel();
            Site = new SiteModel();
        }
    }
}
