using BLL.IServices;
using BLL.Models;
using Client.Models;
using Microsoft.AspNet.Identity;
using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Serilog;

namespace Periodicals.Controllers
{
    /// <summary>
    /// Class that is responsible for processing requests related to manipulating subscriptions
    /// </summary>
    [Authorize]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger _logger;

        public SubscriptionController(ISubscriptionService subscriptionService, ILogger logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
        }
        public ActionResult Index(int page = 1)
        {
            _logger.Information("Action: Index; Controller: Subscription; Call method: GET;");
            try
            {
                string userId = User.Identity.GetUserId();

                int pageSize = 2;
                int totalItems = _subscriptionService.CountSubscriptionsForUser(userId);

                if (page > Math.Ceiling(((double)totalItems / (double)pageSize)) && totalItems != 0)
                {
                    return View("NotFound");
                }

                IEnumerable<Subscription> subscriptionsPerPage = _subscriptionService.GetSubscriptions(userId).OrderBy(s => s.ExpirationDate).Skip((page - 1) * pageSize).Take(pageSize);
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

                SubscriptionViewModel model = new SubscriptionViewModel { Subscriptions = subscriptionsPerPage, PageInfo = pageInfo };

                return View(model);
            } catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpGet]
        public ActionResult New(int? id)
        {
            _logger.Information("Action: New; Controller: Subscription; Call method: GET;");
            try
            {
                if (id == null)
                {
                    id = 1;
                }
                string publisherToFindId = id.ToString();
                var userId = User.Identity.GetUserId();

                if (_subscriptionService.CheckIfSubscriptionExists(userId, publisherToFindId))
                {
                    return View("AlreadySubscribed");
                }

                if (!_subscriptionService.CheckIfSubscritpionPublisherExists(publisherToFindId))
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
            } catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpPost]
        public ActionResult New(SubscriptionCreateViewModel model)
        {
            _logger.Information("Action: New; Controller: Subscription; Call method: POST;");
            try
            {
                if (!ModelState.IsValid)
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
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        public ActionResult Remove(int? id)
        {
            _logger.Information("Action: Remove; Controller: Subscription; Call method: POST;");
            try
            {
                if (id == null)
                {
                    return View("NotFound");
                }

                var subscriptionId = id.ToString();
                _subscriptionService.RemoveExistingSubscription(subscriptionId);

                return RedirectToAction("Index", "Subscription");
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