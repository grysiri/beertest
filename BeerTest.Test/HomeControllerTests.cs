using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeerTest.Controllers;
using BeerTest.DAL;
using BeerTest.Models;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Moq;
using System.Web.Routing;
using System.Security.Principal;

namespace BeerTest.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void HomeController_Index_Returns_beers_with_ratings()
        {
            var mockContext = GetMockContext();

            var controller = new HomeController(mockContext.Object);
            var result = controller.Index() as ViewResult;
            Assert.IsInstanceOfType(result.Model, typeof(IOrderedEnumerable<Beer>));
            var beerlist = (IOrderedEnumerable<Beer>)result.ViewData.Model;
            Assert.AreEqual(2, beerlist.Count());
        }

        [TestMethod]
        public void HomeController_Index_Returns_Score_with_two_decimals()
        {
            var mockContext = GetMockContext();
            var beerWithRatings = mockContext.Object.Beers.Where(b => b.Ratings.Any()).FirstOrDefault();
            var total = 0;
            foreach (var r in beerWithRatings.Ratings)
                total += r.Score;
            double score = (double)total / (double)beerWithRatings.Ratings.Count;

            var controller = new HomeController(mockContext.Object);
            var result = controller.Index() as ViewResult;
            var beerlist = (IOrderedEnumerable<Beer>)result.ViewData.Model;
            Assert.AreEqual(beerWithRatings.ID, beerlist.First().ID);
            Assert.AreNotEqual(score, beerlist.First().Score);
            Assert.AreEqual(Math.Round(score, 2), beerlist.First().Score);
        }

        public Mock<IBeerContext> GetMockContext()
        {
            var mockContext = new Mock<IBeerContext>();
            var beerMockSet = new MockDbSet<Beer>();
            beerMockSet.Object.Add(new Beer { ID = 1, Name = "Testbeer", Brewery = "TestBrewery", Percent = 3, Ratings = new List<Rating>() });
            beerMockSet.Object.Add(new Beer { ID = 2, Name = "Testbeer", Brewery = "TestBrewery", Percent = 6, Ratings = new List<Rating>()
            {
                new Rating { BeerID = 2, Score = 3 },
                new Rating { BeerID = 2, Score = 7 },
                new Rating { BeerID = 2, Score = 7 }
            } });
            mockContext.Setup(c => c.Beers).Returns(beerMockSet.Object);
            var ratingMockSet = new MockDbSet<Rating>();
            mockContext.Setup(c => c.Ratings).Returns(ratingMockSet.Object);
            return mockContext;
        }

        class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class
        {
            public ICollection<TEntity> BackingStore { get; set; }

            public MockDbSet()
            {
                var queryable = (this.BackingStore ?? (this.BackingStore = new List<TEntity>())).AsQueryable();

                this.As<IQueryable<TEntity>>().Setup(e => e.Provider).Returns(queryable.Provider);
                this.As<IQueryable<TEntity>>().Setup(e => e.Expression).Returns(queryable.Expression);
                this.As<IQueryable<TEntity>>().Setup(e => e.ElementType).Returns(queryable.ElementType);
                this.As<IQueryable<TEntity>>().Setup(e => e.GetEnumerator()).Returns(() => queryable.GetEnumerator());

                // Mock the insertion of entities
                this.Setup(e => e.Add(It.IsAny<TEntity>())).Returns((TEntity entity) =>
                {
                    this.BackingStore.Add(entity);

                    return entity;
                });
            }
        }
    }
}
