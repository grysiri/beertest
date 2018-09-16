using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BeerTest.Models;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace BeerTest.DAL
{
    public interface IBeerContext
    {
        IDbSet<Beer> Beers { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<Rating> Ratings { get; set; }
    }
}