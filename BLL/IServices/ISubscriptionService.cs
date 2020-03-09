using BLL.Models;
using System;
using System.Collections.Generic;

namespace BLL.IServices
{
    public interface ISubscriptionService
    {
        List<Subscription> GetSubscriptions(string userId);
        int CountSubscriptions();
        bool CheckIfSubscriptionExists(string userId, string publisherId);
        Publisher GetSubscriptionPublisher(string publisherId);
        int GetUserBalanceForSubscription(string userId);
        void RegisterNewSubscription(string userId, string publisherId, string subscriptionPeriod);
    }
}
