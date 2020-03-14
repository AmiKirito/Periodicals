using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string exceptionType, string exceptionMessage)
        {
            return View();
        }
    }
}