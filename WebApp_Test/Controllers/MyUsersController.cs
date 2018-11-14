using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp_Test.Models;

namespace WebApp_Test.Controllers
{
    public class MyUsersController : Controller
    {
        private DB db = new DB();

    
        public ActionResult Index()
        {
            return View(db.MyUsers.ToList());
        }

         public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUsers myUsers = db.MyUsers.Find(id);
            if (myUsers == null)
            {
                return HttpNotFound();
            }
            return View(myUsers);
        }

         public ActionResult Create()
        {
            return View();
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MyUsers myUsers)
        {
            if (ModelState.IsValid)
            {
                db.MyUsers.Add(myUsers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myUsers);
        }

         public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUsers myUsers = db.MyUsers.Find(id);
            if (myUsers == null)
            {
                return HttpNotFound();
            }
            return View(myUsers);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MyUsers myUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myUsers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myUsers);
        }

         public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyUsers myUsers = db.MyUsers.Find(id);
            if (myUsers == null)
            {
                return HttpNotFound();
            }
            return View(myUsers);
        }

         [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            MyUsers myUsers = db.MyUsers.Find(id);
            db.MyUsers.Remove(myUsers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		 private bool IsExist(long id)
        {
            return db.MyUsers.Any(e => e.Id == id);
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
