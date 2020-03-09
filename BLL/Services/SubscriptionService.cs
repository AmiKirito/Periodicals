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
        public int CountSubscriptions()
        {
            return _subscriptionRepository.CountAll();
        }

        public List<Subscription> GetSubscriptions(string userId)
        {
            return _subscriptionRepository.GetAll(userId);
        }
    }
}
