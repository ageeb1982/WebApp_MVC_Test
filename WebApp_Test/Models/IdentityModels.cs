using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp_Test.Models.Tools;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Test.Models
{
    /// <summary>
    /// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// </summary>

    public class MyUsers : IdentityUser
    {
       /// <summary>
       /// GenerateUserIdentityAsync
       /// </summary>
       /// <param name="manager"></param>
       /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<MyUsers> manager)
        {
            
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// كلاس الإتصال بقاعدة البينات
    /// </summary>
    public class DB : IdentityDbContext<MyUsers>
    {
        /// <summary>
        /// متغير الموضوع
        /// </summary>
        public DbSet<article> articles { set; get; }
        
        /// <summary>
        /// دالة بداية تشغيل قاعدة البيانات
        /// </summary>
        public DB()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// دالة إنشاء قاعدة البيانات
        /// </summary>
        /// <returns></returns>
        public static DB Create()
        {
            return new DB();
        }

    }
}