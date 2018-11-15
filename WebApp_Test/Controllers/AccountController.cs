using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApp_Test.Models;
using WebApp_Test.Models.Tools;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApp_Test.Controllers
{/// <summary>
/// كونترول المستخدمين
/// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        /// <summary>
        /// دالة الإنشاء
        /// </summary>
        public AccountController()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        /// <summary>
        /// 
        /// </summary>
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        /// GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        /// POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await CreateRole_AND_User();


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                //case SignInStatus.LockedOut:
                //    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        /// <summary>
        /// دالة تقوم بإنشاء الوظائف والمستخدمين 
        /// ثم تضيف وظيفة كل مستخدم على حدى
        /// Admin and User
        /// </summary>
        /// <returns></returns>
        private async Task CreateRole_AND_User()
        {

            #region  Create Role/Type of Users
            foreach (Users_Type type in Enum.GetValues(typeof(Users_Type)))
            {

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new DB()));

                //  IdentityRole Rol;
                //string RolName;
                //string SS = nameof(Users_Type.Admin);
                string RolName = type.ToString();
                // Rol = new IdentityRole(RolName);
                try
                {
                    if (!await roleManager.RoleExistsAsync(RolName))
                    {
                        //roleManager.Create(Rol);
                        roleManager.Create(new IdentityRole(RolName));
                        //roleManger.Roles.
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            #endregion

            #region Add Users
            
            if (!SignInManager.UserManager.Users.Any(x => x.UserName == "Admin"))
            {
                var user = new MyUsers { UserName = "Admin", Email = "Admin@a.com" };
                var Create_User = await UserManager.CreateAsync(user, "Password");
                if (Create_User.Succeeded)
                {
                    UserManager.AddToRole(user.Id, nameof(Users_Type.Admin));
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                }
            }

            if (!SignInManager.UserManager.Users.Any(x => x.UserName == "User"))
            {
                var user = new MyUsers { UserName = "User", Email = "UserX@a.com" };
                var Create_User = await UserManager.CreateAsync(user, "Password");
                if (Create_User.Succeeded)
                {
                    UserManager.AddToRole(user.Id, nameof(Users_Type.Articles_Viewer));
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                }
            }

            #endregion
        }

        /// <summary>
        /// دالة تسجيل الخروج
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }



        /// <summary>
        /// إنهاء الكنترولر
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}