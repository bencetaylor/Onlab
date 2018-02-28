using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRent.Models.CarRentModels
{
    public class CommentModel
    {
        [Key]
        public int CommentID { get; set; }
        public String Text { get; set; }

    }
}
