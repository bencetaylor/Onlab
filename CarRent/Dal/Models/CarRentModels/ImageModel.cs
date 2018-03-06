using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.DAL.Models.CarRentModels
{
    public class ImageModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }
        public String Name { get; set; }
        public byte[] Content { get; set; }
        public CarModel Car { get; set; }

        public ImageModel()
        {
            Car = new CarModel();
        }
    }
}
