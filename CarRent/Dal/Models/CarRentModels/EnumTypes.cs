using System;
using System.Collections.Generic;
using System.Text;

namespace CarRent.DAL.Models.CarRentModels
{
    public class EnumTypes
    {
        public enum CarType { Sedan, Hatchback, SUV, Crossover, Pickup }
        public enum CarState { Rented = 1, Available = 2, InService = 3 };
        public enum InsuranceType { Basic = 1, Gold = 2, Platinum = 3 }
        public enum RentState { Approved = 1, Dismissed = 2, Pending = 3 }
    }
}
