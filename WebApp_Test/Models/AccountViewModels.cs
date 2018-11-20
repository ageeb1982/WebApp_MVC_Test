using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp_Test.Models.Tools;

namespace WebApp_Test.Models
{
    
    /// <summary>
    /// الدخول
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// اسم المستخدم
        /// </summary>
        [Required]
        [Display(Name = "User Name")]
         
        public string UserName { get; set; }
        /// <summary>
        /// كلمة السر 
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        /// <summary>
        ///إعادة كلمة السر 
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    /// <summary>
    /// تسجيل مستخدم جديد
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// id For Edit Action
        /// </summary>
        public string Id { set; get; }
        /// <summary>
        /// اسم المستخدم
        /// </summary>
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }



        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [Required]
        [Display(Name = "Email Adress")]
        public string Email { get; set; }
        
        
        /// <summary>
        /// كلمة السر
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }



        /// <summary>
        /// إعادة كلمة السر
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// صلاحية المستخدم
        /// </summary>
        [Display(Name ="User Role")]
        [Required(ErrorMessage ="Please Select Role")]
        public Users_Type user_Type { set; get; }

        /// <summary>
        /// التحويل التلقائي من 
        /// MyUser => RegisterViewModel
        /// </summary>
        /// <param name="user"></param>
        public static implicit  operator RegisterViewModel(MyUsers user)
        {
            var _Reg = new RegisterViewModel();
            _Reg.Id = user.Id;
            _Reg.UserName = user.UserName;
            _Reg.Email = user.Email;
            return _Reg;



        }

    }
     
}
