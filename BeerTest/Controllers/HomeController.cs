using BeerTest.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BeerTest.Controllers
{
    public class HomeController : Controller
    {
        private IBeerContext db;

        public HomeController()
        {
            db = new BeerContext();
        }

        public HomeController(IBeerContext context)
        {
            db = context;
        }

        public ActionResult Index()
        {
            var beers = db.Beers.OrderByDescending(b => b.Sorting).ToList();
            foreach (var beer in beers)
                foreach (var rating in db.Ratings.Where(r => r.BeerID == beer.ID))
                    beer.Ratings.Add(rating);
            
            return View(beers.OrderByDescending(b=>b.Score));
        }

        public ActionResult Register()
        {
            ViewBag.Message = "Unknown user?";

            return View();
        }

        public ActionResult RenderComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ratings = db.Ratings.Where(r => r.BeerID == id).ToList();
            foreach (var r in ratings)
            {
                r.User = db.Users.FirstOrDefault(u => u.ID == r.UserID);
            }
            return View(ratings);
        }

    }
}