using BLL.IServices;
using BLL.Models;
using Client.Models;
using Microsoft.AspNet.Identity;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

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
            string userId = User.Identity.GetUserId();

            int pageSize = 2;
            int totalItems = _subscriptionService.CountSubscriptionsForUser(userId);

            if (page > Math.Ceiling(((double)totalItems / (double)pageSize)) && totalItems != 0)
            {
                return View("NotFound");
            }

            IEnumerable<Subscription> subscriptionsPerPage = _subscriptionService.GetSubscriptions(userId).Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

            SubscriptionViewModel model = new SubscriptionViewModel { Subscriptions = subscriptionsPerPage, PageInfo = pageInfo };

            return View(model);
        }
        [HttpGet]
        public ActionResult New(int? id)
        {
            if(id == null)
            {
                id = 1;
            }
            string publisherToFindId = id.ToString();
            var userId = User.Identity.GetUserId();

            if (_subscriptionService.CheckIfSubscriptionExists(userId, publisherToFindId))
            {
                return View("AlreadySubscribed");
            }

            if(!_subscriptionService.CheckIfSubscritpionPublisherExists(publisherToFindId))
            {
                return View("NotFound");
            }

            SubscriptionCreateViewModel model = new SubscriptionCreateViewModel
            {
                Publisher = _subscriptionService.GetSubscriptionPublisher(publisherToFindId),
                UserId = User.Identity.GetUserId(),
                UserBalance = _subscriptionService.GetUserBalanceForSubscription(User.Identity.GetUserId())
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult New(SubscriptionCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.TotalPrice > model.UserBalance)
            {
                ModelState.AddModelError("", "Not enough money to complete the operation");
                return View(model);
            }

            _subscriptionService.RegisterNewSubscription(model.UserId, model.Publisher.Id, model.SubscriptionPeriod);

            return RedirectToAction("Index", "Subscription");
        }
    }
}