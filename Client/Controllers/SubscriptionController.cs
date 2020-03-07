using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class SubscriptionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}