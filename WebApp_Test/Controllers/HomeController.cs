using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_Test.Controllers
{
    /// <summary>
    /// الصفحة الرئيسية
    /// </summary>
    public class HomeController : Controller
    {   /// <summary>
    /// الصفحة الرئيسية
    /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// من نحن
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        /// <summary>
        /// اتصل بنا
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}