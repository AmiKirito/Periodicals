using System;

namespace BLL.Models
{
    public class Subscription
    {
        public string Id { get; set; }
        public int Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public string SubscriptionPeriod { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsExpired { get; set; }
        public bool IsRemoved { get; set; }
        public Subscription() { }
        public Subscription(string id, int price, DateTime expirationDate, string publisherId, string subscriptionPeriod, string userId,
                            bool isRemoved, bool isExpired)
        {
            Id = id;
            Price = price;
            ExpirationDate = expirationDate;
            PublisherId = publisherId;
            SubscriptionPeriod = subscriptionPeriod;
            UserId = userId;
            IsExpired = isExpired;
            IsRemoved = isRemoved;
        }        
    }
}
