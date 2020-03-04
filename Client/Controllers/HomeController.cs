using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Periodicals.App_Start;

namespace Periodicals.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var user = userManager.FindByName("AmiKirito");
            signInManager.PasswordSignIn(user.UserName, "qwerty123456", true, false);

            var signInStatus = signInManager.PasswordSignIn(user.UserName, "qwerty123456", true, false);
            if (signInStatus == SignInStatus.Success)
            {
                ViewBag.LoginStatus = "Login success";
                return View();
            }
            else
            {
                ViewBag.LoginStatus = "Login failed";
                return View();
            }
        }
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Test()
        {
            return View();
        }
    }
}