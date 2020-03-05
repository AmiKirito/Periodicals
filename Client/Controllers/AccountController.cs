using Client.ViewModels;
using DAL.ModelsEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Periodicals.App_Start;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public ApplicationUserManager UserManager 
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationSignInManager>();
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        public IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var result = SignInManager.PasswordSignIn(model.Username, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new UserEntity()
            {
                UserName = model.Username,
                Email = model.Email
            };
            var result = UserManager.Create(user, model.Password);
            
            if(!result.Succeeded)
            {
                ModelState.AddModelError("errorAttempt", "Invalid registration attempt");
                return View(model);
            }

            var signIn = SignInManager.PasswordSignIn(user.UserName, model.Password, false, true);
            switch (signIn)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
    }
}