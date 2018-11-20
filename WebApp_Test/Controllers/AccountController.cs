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
using System.Data.Entity;
using System.Collections.Generic;

namespace WebApp_Test.Controllers
{/// <summary>
/// كونترول Users
/// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        DB db = new DB();
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
        /// دالة تقوم بإنشاء الوظائف وUsers 
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
        /// Show All User
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
        public ActionResult Index()
        {

            return View(db.Users.ToList());
        }
        /// <summary>
        /// Register New User
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
        public ActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Register New User (post)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = nameof(Users_Type.Admin))]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //لكي لا يتكرر اسم المستخدم
            if (UserManager.Users.Any(m => m.UserName.Trim().ToLower() == model.UserName.Trim().ToLower()))
            {
                ModelState.AddModelError("User_Name", "Duplicate User name -  The user name already exists please try Again");
            }

            //لكي لا يتكرر البريد الإلكتروني
            if (UserManager.Users.Any(x =>  x.Email.Trim().ToLower() == model.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email", "Duplicate Email Address -  The Email Adress already exists please try Again");
            }


            if (ModelState.IsValid)
            {
                UserManager.UserValidator = new UserValidator<MyUsers>(UserManager)
                {
                    AllowOnlyAlphanumericUserNames = false,

                };
                var user = new MyUsers
                {
                    UserName = model.UserName,
                    Email = model.Email

                };






                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    UserManager.AddToRole(user.Id, model.user_Type.ToString());


                    return RedirectToAction("Index");
                }
                AddErrors(result);


            }
            return View(model);
        }

 /// <summary>
        /// edit for User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
        public ActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            MyUsers usr = db.Users.Find(id);

            Users_Type UsrType = Users_Type.Articles_Viewer;


            if (UserManager.IsInRole(usr.Id, Users_Type.Admin.ToString()))
            {
                UsrType = Users_Type.Admin;
            }

            RegisterViewModel dee = usr;
            dee.user_Type = UsrType;
            return View(dee);
        }


        /// <summary>
        /// edit Profile Of User (post)
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        [Authorize()]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit_Prof(RegisterViewModel prof)
        {
            //لكي لا يتكرر البريد الإلكتروني
             if (UserManager.Users.Any(x => x.Id != prof.Id && x.Email.Trim().ToLower() == prof.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email","Duplicate Email Address -  The Email Adress already exists please try Again");
            }

                      

            if (ModelState.IsValid)
            {


                var usr = db.Users.FirstOrDefault(x => x.Id == prof.Id);
                if (usr != null)
                {
                    usr.Email = prof.Email;
                    
                     
                    db.Entry(usr).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    var mo = await UserManager.GeneratePasswordResetTokenAsync(usr.Id);
                    var result = await UserManager.ResetPasswordAsync(usr.Id, mo, prof.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                return  RedirectToAction("Index", "Home" );
            }
            return View(prof);
        }


        /// <summary>
        /// edit Profile for User
        /// </summary>
     
        /// <returns></returns>
        [Authorize()]
        public ActionResult Edit_Prof()
        {

            //MyUsers usr = reg.GetUser();

            string Usr_Id = User.Identity.GetUserId();
            MyUsers dd = db.Users.FirstOrDefault(x => x.Id == Usr_Id);
            if (dd != null)
            {
                RegisterViewModel user = dd;

                return View(user);
            }
            return RedirectToAction("Home","Index");
        }


        /// <summary>
        /// edit for user (post)
        /// </summary>
        /// <param name="edit_Usr"></param>
        /// <returns></returns>
        [Authorize(Roles = nameof(Users_Type.Admin))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RegisterViewModel edit_Usr)
        {
            //لكي لا يتكرر البريد الإلكتروني
             if (UserManager.Users.Any(x => x.Id != edit_Usr.Id && x.Email.Trim().ToLower() == edit_Usr.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email","Duplicate Email Address -  The Email Adress already exists please try Again");
            }

             //لحذف المطالبة بكلمة السر
           ModelState["Password"].Errors.Clear();  
                    

            if (ModelState.IsValid)
            {


                var usr = db.Users.FirstOrDefault(x => x.Id == edit_Usr.Id);
                if (usr != null)
                {
                    usr.Email = edit_Usr.Email;

                    var roles = UserManager.GetRoles(usr.Id).ToArray();
                    UserManager.RemoveFromRoles(usr.Id, roles);

                    if(usr.UserName=="Admin")
                    {
                        edit_Usr.user_Type = Users_Type.Admin;
                    }


                    db.Entry(usr).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    await UserManager.AddToRoleAsync(usr.Id, edit_Usr.user_Type.ToString());
                }
                return  RedirectToAction("Index", "Account" );
            }
            return View(edit_Usr);
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