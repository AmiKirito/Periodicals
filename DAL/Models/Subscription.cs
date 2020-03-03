using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Subscription
    {
        public string Id { get; set; }
        public int Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public Period SubscriptionPeriod{ get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public enum Period
        {
            month = 1,
            quarter = 3,
            half = 6,
            year = 12
        }
    }
}
