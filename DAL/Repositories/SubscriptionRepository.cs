using BLL.IRepositories;
using BLL.Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext context)
        {
            _context = context;
        }
        public int CountAll()
        {
            var subscriptionsCount = _context.Subscriptions.Count();
            return subscriptionsCount;
        }

        public List<Subscription> GetAll(string userId)
        {
            var subscriptionsQuery = _context.Subscriptions.Include("Publisher").ToList();
            var subscriptions = new List<Subscription>();

            foreach (var subscriptionEntity in subscriptionsQuery)
            {
                var subscription = new Subscription();
                var publisher = new Publisher();

                publisher.Id = subscriptionEntity.Publisher.Id;
                publisher.Title = subscriptionEntity.Publisher.Title;
                publisher.Desription = subscriptionEntity.Publisher.Description;
                publisher.IsRemoved = subscriptionEntity.Publisher.IsRemoved;

                subscription.Id = subscriptionEntity.Id;
                subscription.Price = subscriptionEntity.Price;
                subscription.ExpirationDate = subscriptionEntity.ExpirationDate;
                subscription.PublisherId = publisher.Id;
                subscription.Publisher = publisher;
                subscription.SubscriptionPeriod = subscriptionEntity.SubscriptionPeriod;
                subscription.UserId = userId;
                subscription.IsExpired = subscriptionEntity.IsExpired;

                subscriptions.Add(subscription);
            }
            return subscriptions;
        }
    }
}
