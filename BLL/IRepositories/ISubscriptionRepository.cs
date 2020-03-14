using BLL.Models;
using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface ISubscriptionRepository
    {
        List<Subscription> GetAll(string userId);
        int CountAllForUser(string userId);
        bool CheckForSubscription(string userId, string publisherId);
        Publisher GetPublisherForSubscription(string publisherId);
        bool CheckForSubscriptionPublisher(string publisherId);
        int GetUserBalance(string userId);
        void LinkNewSubscription(string userId, string publisherId, string subscriptionPeriod);
    }
}
