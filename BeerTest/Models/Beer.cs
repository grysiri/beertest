using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BeerTest.Models
{
    public class Beer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Brewery { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        [DisplayFormat(DataFormatString=("{00,0}"))]
        public decimal Percent { get; set; }
        public int Sorting { get; set; }
        public string FullName { get { return Brewery + " " + Name; } }

        public virtual double? Score 
        { 
            get 
            {
                if (Ratings == null || !Ratings.Any())
                    return null;
                var total = 0;
                foreach (var r in Ratings)
                    total += r.Score;
                double score = (double)total / (double)Ratings.Count;
                return Math.Round(score, 2);
            } 
        }
        public ICollection<Rating> Ratings { get; set; }
    }
}