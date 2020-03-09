using BLL.IRepositories;
using BLL.Models;
using DAL.ModelsEntities;
using System;
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

        public bool CheckForSubscription(string userId, string publisherId)
        {
            bool doesExist = false;

            SubscriptionEntity result = new SubscriptionEntity();

            try
            {
                result = _context.Subscriptions.Where(s => s.UserId == userId && s.PublisherId == publisherId).First();
            } catch { }

            if(!string.IsNullOrEmpty(result.Id))
            {
                doesExist = true;
            }

            return doesExist;
        }

        public int CountAll()
        {
            var subscriptionsCount = _context.Subscriptions.Count();
            return subscriptionsCount;
        }

        public List<Subscription> GetAll(string userId)
        {
            var subscriptionsQuery = _context.Subscriptions.Include("Publisher").Where(s => s.UserId == userId).ToList();
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

                if(subscriptionEntity.ExpirationDate > DateTime.UtcNow)
                {
                    subscription.IsExpired = false;
                }
                else
                {
                    subscription.IsExpired = true;
                }

                subscriptions.Add(subscription);
            }
            return subscriptions;
        }
        public Publisher GetPublisherForSubscription(string publisherId)
        {
            var subscriptionPublisherEntity = _context.Publishers.Include("Authors").Where(p => p.Id == publisherId).First();
            Publisher subscriptionPublisher = new Publisher
            {
                Id = subscriptionPublisherEntity.Id,
                Title = subscriptionPublisherEntity.Title,
                Desription = subscriptionPublisherEntity.Description,
                MonthlySubscriptionPrice = subscriptionPublisherEntity.MonthlySubscriptionPrice,
                IsRemoved = subscriptionPublisherEntity.IsRemoved
            };

            return subscriptionPublisher;

        }

        public int GetUserBalance(string userId)
        {
            var userBalance = _context.Users.OfType<UserEntity>().Where(u => u.Id == userId).First().AccountSum;

            return userBalance;
        }

        public void LinkNewSubscription(string userId, string publisherId, string subscriptionPeriod)
        {
            var subscriptionPublisher = GetPublisherForSubscription(publisherId);

            var subscriptionToAdd = new SubscriptionEntity
            {
                Id = (CountAll() + 1).ToString(),
                UserId = userId,
                PublisherId = publisherId,
                IsExpired = false
            };

            switch (subscriptionPeriod)
            {
                case "month":
                    subscriptionToAdd.SubscriptionPeriod = "Monthly";
                    subscriptionToAdd.Price = subscriptionPublisher.MonthlySubscriptionPrice * 1;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                    break;
                case "quarter":
                    subscriptionToAdd.SubscriptionPeriod = "Quarter year";
                    subscriptionToAdd.Price = subscriptionPublisher.MonthlySubscriptionPrice * 3;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(3);
                    break;
                case "half":
                    subscriptionToAdd.SubscriptionPeriod = "Half year";
                    subscriptionToAdd.Price = subscriptionPublisher.MonthlySubscriptionPrice * 6;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(6);
                    break;
                case "year":
                    subscriptionToAdd.SubscriptionPeriod = "Annual";
                    subscriptionToAdd.Price = subscriptionPublisher.MonthlySubscriptionPrice * 12;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(12);
                    break;
                default:
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                    break;
            }

            _context.Users.OfType<UserEntity>().Where(u => u.Id == userId).First().AccountSum -= subscriptionToAdd.Price;
            _context.Subscriptions.Add(subscriptionToAdd);
            _context.SaveChanges();
        }
    }
}
    