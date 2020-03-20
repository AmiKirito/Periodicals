using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string exceptionType)
        {
            switch (exceptionType)
            {
                case "InvalidOperationException":
                    return View("NotFound");
                default:
                    return View();
            }
        }
        public ActionResult NotFound()
        {
            return View();
        }
    }
}