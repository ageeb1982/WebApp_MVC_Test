using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp_Test.Models.Tools;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp_Test.Models
{
    /// <summary>
    /// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// </summary>

    public class MyUsers : IdentityUser
    {
        /// <summary>
        /// لإضافة الصلاحية للمستخدم
        /// </summary>
        [NotMapped]
        [Display(Name = "User Role")]
       // [Required(ErrorMessage = "Please Select Role")]
        public Users_Type user_Type { set; get; }

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


        /// <summary>
        /// تحويل تلقائي من 
        /// Regisger=>MyUser
        /// </summary>
        /// <param name="_Reg"></param>
        public static implicit operator MyUsers(RegisterViewModel _Reg)
        {
            var _User = new MyUsers();
            _User.Id = _Reg.Id;
            _User.UserName = _Reg.UserName;
            _User.Email = _Reg.Email;
            return _User;



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