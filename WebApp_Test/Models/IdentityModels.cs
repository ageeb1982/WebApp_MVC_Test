using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp_Test.Models.Tools;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Test.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class MyUsers : IdentityUser
    {
       
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

        public DbSet<article> articles { set; get; }
        public DB()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static DB Create()
        {
            return new DB();
        }

    }
}