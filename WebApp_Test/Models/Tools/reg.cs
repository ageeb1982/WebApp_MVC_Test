using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_Test.Models.Tools
{/// <summary>
/// صلاحيات Users
/// </summary>
    public enum Users_Type
    {
        /// <summary>
        /// صلاحية عرض المواضيع فقط
        /// </summary>
        Articles_Viewer = 1,
        /// <summary>
        /// صلاحية المشرف وكامل الصلاحيات
        /// </summary>
        Admin =2
        
    }
    /// <summary>
    /// دالة مساعدة
    /// </summary>
    public class reg
    {
        /// <summary>
        /// دالة تجلب المستخدم الحالي
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public static MyUsers GetUser(string _id = "")
        {
            if (string.IsNullOrEmpty(_id)) _id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            try
            {
                MyUsers user = System.Web.HttpContext.Current.GetOwinContext()
                .GetUserManager<ApplicationUserManager>().FindById(_id);
                  return user;



            }
            catch
            {


                var usr = new MyUsers();
                usr.UserName = "---";
               
                return usr;

            }

        }
    }
}