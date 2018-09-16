using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerTest.Models;
using BeerTest.DAL;

namespace BeerTest.Controllers
{
    public class RatingController : Controller
    {
        private BeerContext db = new BeerContext();

        // GET: /Rating/
        public ActionResult Index()
        {
            var ratings = db.Ratings.Include(r => r.Beer).Include(r => r.User);
            return View(ratings.ToList());
        }

        //GET: /Rating/3
        public ActionResult UserRatings(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var ratings = db.Ratings.Include(r => r.Beer).Include(r => r.User).Where(r => r.UserID == id);
            return View(ratings.ToList());
        }

        // GET: /Rating/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: /Rating/Create
        public ActionResult Create()
        {
            if (Request.Cookies["UserCookie"] == null || Request.Cookies["UserCookie"]["UserID"] == null)
                return RedirectToAction("Create","User");
            var userid = int.Parse(Request.Cookies["UserCookie"]["UserID"]);
            var user = db.Users.FirstOrDefault(u=> u.ID == userid);
            if (user == null)
                return RedirectToAction("Create", "User");
            ViewBag.BeerID = new SelectList(db.Beers.Where(b => !db.Ratings.Any( r => r.UserID == userid && r.BeerID == b.ID)).OrderBy(b=>b.Sorting), "ID", "FullName");
            var list = new List<SelectListItem>();
            for (int i = 1; i < 11; i++)
                list.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            ViewBag.Scorelist = list;
            return View(new Rating{UserID= userid, User = user});
        }

        // POST: /Rating/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,UserID,BeerID,Score,Comment")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Ratings.Add(rating);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            ViewBag.BeerID = new SelectList(db.Beers, "ID", "Name", rating.BeerID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", rating.UserID);
            return View(rating);
        }

        // GET: /Rating/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeerID = new SelectList(db.Beers, "ID", "Name", rating.BeerID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", rating.UserID);
            return View(rating);
        }

        // POST: /Rating/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,UserID,BeerID,Score,Comment")] Rating rating)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BeerID = new SelectList(db.Beers, "ID", "Name", rating.BeerID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", rating.UserID);
            return View(rating);
        }

        // GET: /Rating/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: /Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
