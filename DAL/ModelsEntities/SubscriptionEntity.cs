using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.ModelsEntities
{
    [Table("Subscriptions")]
    public class SubscriptionEntity
    {
        public string Id { get; set; }
        public int Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PublisherId { get; set; }
        public PublisherEntity Publisher { get; set; }
        public string SubscriptionPeriod { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }
        public bool IsExpired { get; set; }
    }
}
