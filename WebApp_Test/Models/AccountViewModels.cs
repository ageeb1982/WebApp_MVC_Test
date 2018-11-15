using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        /// اسم المستخدم
        /// </summary>
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
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
    }
     
}
