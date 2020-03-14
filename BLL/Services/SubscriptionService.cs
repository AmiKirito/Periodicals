using BLL.IRepositories;
using BLL.IServices;
using BLL.Models;
using System.Collections.Generic;

namespace BLL.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public bool CheckIfSubscriptionExists(string userId, string publisherId)
        {
            return _subscriptionRepository.CheckForSubscription(userId, publisherId);
        }

        public bool CheckIfSubscritpionPublisherExists(string publisherId)
        {
            return _subscriptionRepository.CheckForSubscriptionPublisher(publisherId);
        }

        public int CountSubscriptionsForUser(string userId)
        {
            return _subscriptionRepository.CountAllForUser(userId);
        }

        public Publisher GetSubscriptionPublisher(string publisherId)
        {
            return _subscriptionRepository.GetPublisherForSubscription(publisherId);
        }

        public List<Subscription> GetSubscriptions(string userId)
        {
            return _subscriptionRepository.GetAll(userId);
        }

        public int GetUserBalanceForSubscription(string userId)
        {
            return _subscriptionRepository.GetUserBalance(userId);
        }

        public void RegisterNewSubscription(string userId, string publisherId, string subscriptionPeriod)
        {
            _subscriptionRepository.LinkNewSubscription(userId, publisherId, subscriptionPeriod);
        }

        public void RemoveExistingSubscription(string subscriptionId)
        {
            _subscriptionRepository.RemoveSubscription(subscriptionId);
        }
    }
}
