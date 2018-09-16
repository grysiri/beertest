using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeerTest.Controllers;
using BeerTest.DAL;
using BeerTest.Models;
using System.Data.Entity;
using System.Web.Mvc;

namespace BeerTest.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void HomeController_Index_Returns_beers_with_ratings()
        {
            var context = new BeerContext();
            context.Beers.Add(new Beer { Name = "Testbeer", Brewery = "TestBrewery", Percent = 3 });
            var controller = new HomeController(context);
            ActionResult result = controller.Index();
        }
    }
}
