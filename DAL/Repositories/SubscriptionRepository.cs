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
                result = _context.Subscriptions.Where(s => s.UserId == userId && s.PublisherId == publisherId && !(s.IsExpired == true || s.IsRemoved == true)).First();
            } catch { }

            if(!string.IsNullOrEmpty(result.Id))
            {
                doesExist = true;
            }

            return doesExist;
        }
        public int CountAllForUser(string userId)
        {
            var subscriptionsCountForUser = _context.Subscriptions.Where(s => s.UserId == userId && s.IsRemoved == false).Count();
            return subscriptionsCountForUser;
        }
        public List<Subscription> GetAll(string userId)
        {
            var subscriptionsQuery = _context.Subscriptions.Include("Publisher").Where(s => s.UserId == userId && s.IsRemoved == false).ToList();
            var subscriptions = new List<Subscription>();

            foreach (var subscriptionEntity in subscriptionsQuery)
            {
                var subscription = new Subscription();
                Publisher publisher = new Publisher
                {
                    Id = subscriptionEntity.Publisher.Id,
                    Title = subscriptionEntity.Publisher.Title,
                    Description = subscriptionEntity.Publisher.Description,
                    IsRemoved = subscriptionEntity.Publisher.IsRemoved
                };

                subscription.Id = subscriptionEntity.Id;
                subscription.Price = subscriptionEntity.Price;
                subscription.ExpirationDate = subscriptionEntity.ExpirationDate;
                subscription.PublisherId = publisher.Id;
                subscription.Publisher = publisher;
                subscription.SubscriptionPeriod = subscriptionEntity.SubscriptionPeriod;
                subscription.UserId = userId;
                subscription.IsRemoved = subscriptionEntity.IsRemoved;

                if(subscriptionEntity.ExpirationDate > DateTime.UtcNow)
                {
                    subscriptionEntity.IsExpired = false;
                    subscription.IsExpired = false;
                }
                else
                {
                    subscriptionEntity.IsExpired = true;
                    subscription.IsExpired = true;
                }

                _context.SaveChanges();

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
                Description = subscriptionPublisherEntity.Description,
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
        public void PayForSubscription(string userId, int price)
        {
            _context.Users.OfType<UserEntity>().Where(u => u.Id == userId).First().AccountSum -= price;
        }
        public void SetSubscriptionPeriod(SubscriptionEntity subscriptionToAdd, string period, int monthlyPrice)
        {
            switch (period)
            {
                case "month":
                    subscriptionToAdd.SubscriptionPeriod = "Monthly";
                    subscriptionToAdd.Price = monthlyPrice;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                    break;
                case "quarter":
                    subscriptionToAdd.SubscriptionPeriod = "Quarter year";
                    subscriptionToAdd.Price = monthlyPrice * 3;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(3);
                    break;
                case "half":
                    subscriptionToAdd.SubscriptionPeriod = "Half year";
                    subscriptionToAdd.Price = monthlyPrice * 6;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(6);
                    break;
                case "year":
                    subscriptionToAdd.SubscriptionPeriod = "Annual";
                    subscriptionToAdd.Price = monthlyPrice * 12;
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(12);
                    break;
                default:
                    subscriptionToAdd.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                    break;
            }
        }
        public void LinkNewSubscription(string userId, string publisherId, string subscriptionPeriod)
        {
            var subscriptionPublisher = GetPublisherForSubscription(publisherId);

            var subscriptionToAdd = new SubscriptionEntity
            {
                Id = (Convert.ToInt32(_context.Subscriptions.ToList()
                .OrderByDescending(s => Convert.ToInt32(s.Id))
                .ToList().First().Id) + 1).ToString(),
                UserId = userId,
                PublisherId = publisherId,
                IsExpired = false,
                IsRemoved = false
            };

            SetSubscriptionPeriod(subscriptionToAdd, subscriptionPeriod, subscriptionPublisher.MonthlySubscriptionPrice);
            PayForSubscription(userId, subscriptionToAdd.Price);

            _context.Subscriptions.Add(subscriptionToAdd);
            _context.SaveChanges();
        }

        public bool CheckForSubscriptionPublisher(string publisherId)
        {
            if(_context.Publishers.Any(p => p.Id == publisherId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveSubscription(string subscriptionId)
        {
            var subscriptionToRemove = _context.Subscriptions.Where(s => s.Id == subscriptionId).First();
            subscriptionToRemove.IsRemoved = true;

            _context.SaveChanges();
        }
    }
}
    