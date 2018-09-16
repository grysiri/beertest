using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeerTest.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}