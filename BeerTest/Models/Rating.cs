using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BeerTest.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        [Required]
        public int BeerID { get; set; }
        [Required]
        [Range(1,10)]
        public int Score { get; set; }
        public string Comment { get; set; }

        public virtual Beer Beer { get; set; }
        public virtual User User { get; set; }
    }

}