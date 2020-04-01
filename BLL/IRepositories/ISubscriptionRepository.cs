using BLL.Models;
using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface ISubscriptionRepository
    {
        List<Subscription> GetAll(string userId);
        /// <summary>
        /// Method that counts all subscriptions for user and returns their amount
        /// </summary>
        int CountAllForUser(string userId);
        /// <summary>
        /// Method that checks whether the particular subscription for requested user exists
        /// </summary>
        bool CheckForSubscription(string userId, string publisherId);
        /// <summary>
        /// Method that gets the publisher for the subscription in order to take and display required data from it
        /// </summary>
        Publisher GetPublisherForSubscription(string publisherId);
        /// <summary>
        /// Method that checks whether the requested publisher for the subscription exists in order to proceed with purchase further
        /// </summary>
        bool CheckForSubscriptionPublisher(string publisherId);
        /// <summary>
        /// Method that gets the user balance amount in order to use it during the subscription purchase
        /// </summary>
        int GetUserBalance(string userId);
        /// <summary>
        /// Method that links(adds) new subscription to particular publisher
        /// </summary>
        void LinkNewSubscription(string userId, string publisherId, string subscriptionPeriod);
        void RemoveSubscription(string subscriptionId);
    }
}
