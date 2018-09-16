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
    public class UserController : Controller
    {
        private BeerContext db = new BeerContext();

        // GET: /User/
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            User user;
            if (id == null)
            {
                if (Request.Cookies["UserCookie"] == null || Request.Cookies["UserCookie"]["UserID"] == null)
                    return RedirectToAction("Create", "User");
                var userid = int.Parse(Request.Cookies["UserCookie"]["UserID"]);
                user = db.Users.FirstOrDefault(u => u.ID == userid);
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
                user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Create");
            }

            user.Ratings = db.Ratings.Where(r => r.UserID == user.ID).ToList();
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Username")] User user)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Users.FirstOrDefault(u => u.Username.Equals(user.Username));
                if (existing == null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                    user = existing;
                CreateCookie(user);
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Username")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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

        private void CreateCookie(User user)
        {
            var cookie = new HttpCookie("UserCookie");
            cookie["UserID"] = user.ID.ToString();
            cookie["Username"] = user.Username;
            cookie.Expires = DateTime.Now.AddDays(365);
            Response.Cookies.Add(cookie);
        }
    }
}
