using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp_Test.Models.Tools
{
    public enum Users_Type
    {
        Unknown=0,
        Articles_Viewer = 1,
        Admin =2
        
    }
    public class reg
    {
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
                usr.User_TypeX = Users_Type.Unknown;
                return usr;

            }

        }
    }
}