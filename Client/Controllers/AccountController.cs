using Client.ViewModels;
using DAL.ModelsEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Client.App_Start;
using System.Web;
using System.Web.Mvc;
using BLL.IServices;

namespace Periodicals.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
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
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = SignInManager.PasswordSignIn(model.Username, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    if(string.IsNullOrWhiteSpace(ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(ReturnUrl);
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
                ModelState.AddModelError("errorRegisterAttempt", "Invalid registration attempt");
                return View(model);
            }

            var userId = UserManager.FindByName(user.UserName).Id;

            var roleResult = UserManager.AddToRole(userId, "CommonUser");

            if(!roleResult.Succeeded)
            {
                ModelState.AddModelError("errorRegisterAttempt", "Invalid registration attempt");
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
                    ModelState.AddModelError("loginError", "Invalid login attempt.");
                    return View(model);
            }
        }
        public ActionResult Cabinet()
        {
            return View();
        }
        public ActionResult Balance()
        {
            BalanceViewModel model = new BalanceViewModel();
            model.Balance = UserManager.FindById(User.Identity.GetUserId()).AccountSum;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddBalance(BalanceViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Balance");
            }

            _accountService.AddSumToBalance(model.AddSum, User.Identity.Name);

            return RedirectToAction("Balance");
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserId = User.Identity.GetUserId();
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("changeError", "Invalid data entered.");
                return View(model);
            }

            var result = UserManager.ChangePassword(model.UserId, model.OldPassword, model.NewPassword);
            switch (result.Succeeded)
            {
                case true:
                    return View("Cabinet");
                default:
                    ModelState.AddModelError("", "Invalid change password attempt.");
                    return View(model);
            }
        }
    }
}