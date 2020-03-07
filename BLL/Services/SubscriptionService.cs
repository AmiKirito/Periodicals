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
        public List<Subscription> GetSubscriptions()
        {
            return _subscriptionRepository.GetAll();
        }
    }
}
