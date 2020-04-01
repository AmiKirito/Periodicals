using System.Web.Mvc;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Class that is responsible for processing exceptions and errors which occur during request processing pipeline
    /// </summary>
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