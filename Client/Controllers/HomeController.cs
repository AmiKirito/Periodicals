using BLL.IServices;
using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using BLL.Models;
using System.Linq;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPublisherService _publisherService;
        public HomeController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }
        [AllowAnonymous]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 2;

            IEnumerable<Publisher> publishersPerPage = _publisherService.GetAll().Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = _publisherService.CountAll() };
            PublisherViewModel model = new PublisherViewModel { PageInfo = pageInfo, Publishers = publishersPerPage };

            return View(model);
        }
    }
}