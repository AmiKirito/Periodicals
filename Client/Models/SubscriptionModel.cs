using BLL.EnumExtensions;
using System;

namespace Client.Models
{
    public class SubscriptionModel
    {
        public string Id { get; set; }
        public int Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PublisherId { get; set; }
        public PublisherModel Publisher { get; set; }
        public string SubscriptionPeriod
        {
            get { return SubscriptionPeriod; }
            private set { SubscriptionPeriod = EnumExtensions.ParseEnumValue<Period>(value).ToString(); }
        }
        public string UserId { get; set; }
        public UserModel User { get; set; }

        public enum Period
        {
            month = 1,
            quarter = 3,
            half = 6,
            year = 12
        }
        
    }
}
