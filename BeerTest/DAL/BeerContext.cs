using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using BeerTest.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BeerTest.DAL
{
    public class BeerContext : DbContext
    {
        public BeerContext() : base("BeerContext")
        {
            Database.SetInitializer<BeerContext>(new CreateDatabaseIfNotExists<BeerContext>());
        }

        public DbSet<Beer> Beers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Beer>().Property(o => o.Percent).HasPrecision(3, 1);

            base.OnModelCreating(modelBuilder);
        }
    }
}