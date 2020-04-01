using Client.ViewModels;
using DAL.ModelsEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Client.App_Start;
using System.Web;
using System.Web.Mvc;
using BLL.IServices;
using Serilog;
using System;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Class that is responsible for processing requests which are related to manipulating user account
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger _logger;

        public AccountController(IAccountService accountService, ILogger logger)
        {
            _accountService = accountService;
            _logger = logger;
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
            _logger.Information("Action: Login; Controller: Account; Call method: GET;");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model, string ReturnUrl)
        {
            _logger.Information("Action: Login; Controller: Account; Call method: POST;");
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = SignInManager.PasswordSignIn(model.Username, model.Password, model.RememberMe, shouldLockout: true);
                switch (result)
                {
                    case SignInStatus.Success:
                        if (string.IsNullOrWhiteSpace(ReturnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        return Redirect(ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }

        [Authorize]
        public ActionResult LogOut()
        {
            _logger.Information("Action: LogOut; Controller: Account; Call method: POST;");
            try
            {
                AuthenticationManager.SignOut();

                return RedirectToAction("Login", "Account");
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            _logger.Information("Action: Register; Controller: Account; Call method: GET;");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            _logger.Information("Action: Register; Controller: Account; Call method: POST;");
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = new UserEntity()
                {
                    UserName = model.Username,
                    Email = model.Email
                };
                var result = UserManager.Create(user, model.Password);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid registration attempt");
                    return View(model);
                }

                var userId = UserManager.FindByName(user.UserName).Id;

                var roleResult = UserManager.AddToRole(userId, "CommonUser");

                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid registration attempt");
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
                        ModelState.AddModelError("", "Invalid login attempt");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        public ActionResult Cabinet()
        {
            _logger.Information("Action: Cabinet; Controller: Account; Call method: GET;");
            try
            {
                return View();
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        public ActionResult Balance()
        {
            _logger.Information("Action: Balance; Controller: Account; Call method: GET;");
            try
            {
                BalanceViewModel model = new BalanceViewModel();
                model.Balance = UserManager.FindById(User.Identity.GetUserId()).AccountSum;
                return View(model);
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpPost]
        public ActionResult AddBalance(BalanceViewModel model)
        {
            _logger.Information("Action: AddBalance; Controller: Account; Call method: POST;");
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Balance");
                }

                _accountService.AddSumToBalance(model.AddSum, User.Identity.Name);

                return RedirectToAction("Balance");
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            _logger.Information("Action: ChangePassword; Controller: Account; Call method: GET;");
            try
            {
                ChangePasswordViewModel model = new ChangePasswordViewModel();
                model.UserId = User.Identity.GetUserId();
                return View(model);
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            _logger.Information("Action: ChangePassword; Controller: Account; Call method: POST;");
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("changeError", "Invalid data entered");
                    return View(model);
                }
                var result = UserManager.ChangePassword(model.UserId, model.OldPassword, model.NewPassword);
                switch (result.Succeeded)
                {
                    case true:
                        return View("Cabinet");
                    default:
                        ModelState.AddModelError("", "Invalid change password attempt");
                        return View(model);
                }
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        private void LogException(Exception e)
        {
            var exceptionType = e.GetType().Name;
            var exceptionMessage = e.Message;
            var stackTrace = e.StackTrace.ToString();
            var source = e.Source.ToString();

            _logger.Information($"Exception: Type - {exceptionType}; Message - {exceptionMessage};" +
                $" StackTrace - {stackTrace};" +
                $" Source - {source};");
        }
    }
}