using BLL.IServices;
using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using BLL.Models;
using System.Linq;
using System.Web.Mvc;
using System;

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
            int totalItems = _publisherService.CountPublishers();

            if (page > Math.Ceiling(((double)totalItems / (double)pageSize)))
            {
                return View("NotFound");
            }
            IEnumerable<Publisher> publishersPerPage = _publisherService.GetPublishers().Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

            PublisherViewModel model = new PublisherViewModel { PageInfo = pageInfo, Publishers = publishersPerPage };

            return View(model);
        }
    }
}