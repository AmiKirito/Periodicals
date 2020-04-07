using System;
using Serilog;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Class that is responsible for processing requests related to the home page
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController() { }
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

                return RedirectToAction("Index", "Error", new { exceptionType = e.GetType().Name });
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