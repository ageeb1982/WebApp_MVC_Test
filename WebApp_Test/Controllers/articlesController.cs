using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp_Test.Models;
using WebApp_Test.Models.Tools;

namespace WebApp_Test.Controllers
{

    [Authorize]
    public class articlesController : Controller
    {
        private DB db = new DB();
        Users_Type UserT = reg.GetUser().User_TypeX;

        public ActionResult Index()
        {

            if (UserT == Users_Type.Unknown) { return RedirectToAction("Login", "Account", new { ReturnUrl = Request.Url.LocalPath }); }
            return View(db.articles.ToList());
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            article article = db.articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        public ActionResult Create()
        {
            if (UserT == Users_Type.Unknown || UserT==Users_Type.Articles_Viewer) { return RedirectToAction("Login", "Account", new { ReturnUrl = Request.Url.LocalPath }); }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(article article)
        {
            if (ModelState.IsValid)
            {
                article.AddTime = DateTime.Now;
                db.articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            article article = db.articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(article article)
        {


            if (ModelState.IsValid)
            {



                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            article article = db.articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            article article = db.articles.Find(id);
            db.articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool IsExist(long id)
        {
            return db.articles.Any(e => e.Id == id);
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
