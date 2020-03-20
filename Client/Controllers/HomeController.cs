using System;
using Serilog;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger logger)
        {
            _logger = logger;
        }
        public ActionResult Index()
        {
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