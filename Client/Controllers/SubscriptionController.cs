using BLL.IServices;
using BLL.Models;
using Client.Models;
using Microsoft.AspNet.Identity;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Periodicals.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
        public ActionResult Index(int page = 1)
        {
            int pageSize = 5;
            int totalItems = _subscriptionService.CountSubscriptions();
            string userId = User.Identity.GetUserId();

            if (page > Math.Ceiling(((double)totalItems / (double)pageSize)))
            {
                return View("NotFound");
            }

            IEnumerable<Subscription> subscriptionsPerPage = _subscriptionService.GetSubscriptions(userId);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

            SubscriptionViewModel model = new SubscriptionViewModel { Subscriptions = subscriptionsPerPage, PageInfo = pageInfo };

            return View(model);
        }
    }
}