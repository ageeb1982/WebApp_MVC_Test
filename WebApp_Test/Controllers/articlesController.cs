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

        /// <summary>
        /// تظهر قائمة المواضيع وهي متاحة لكلا المستخدمين 
        ///Admin & User
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin) + "," + nameof(Users_Type.Articles_Viewer))]
        public ActionResult Index()
        {

            return View(db.articles.ToList());
        }


        /// <summary>
        ///تظهر تفاصيل المواضيع وهي متاحة لكلا المستخدمين
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin) + "," + nameof(Users_Type.Articles_Viewer))]

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



        /// <summary>
        /// تنشئ موضوع جديد لكنها متاحة للمستخدم 
        /// Admin
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
        public ActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = nameof(Users_Type.Admin))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(article article)
        {

            if(db.articles.Any(x=>x.Name.Trim()== article.Name.Trim()))
            {
                ModelState.AddModelError("Name", "This name has already been entered");
            }

            if (ModelState.IsValid)
            {
                article.AddTime = DateTime.Now;
                db.articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        /// <summary>
        /// تعدل موضوع موجود وهي متاحة للمستخدم
        /// Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
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


        [Authorize(Roles = nameof(Users_Type.Admin))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(article article)
        {


            if (db.articles.Any(x => x.Name.Trim() == article.Name.Trim() && x.Id!=article.Id))
            {
                ModelState.AddModelError("Name", "This name has already been entered");
            }

            if (ModelState.IsValid)
            {



                db.Entry(article).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(article);
        }
        /// <summary>
        /// تحذف موضوع موجود وهي متاحة للمستخدم 
        /// Admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
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

        [Authorize(Roles = nameof(Users_Type.Admin))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            article article = db.articles.Find(id);
            db.articles.Remove(article);
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
