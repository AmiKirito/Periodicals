using System.Collections.Generic;

namespace Client.Models
{
    public class UserModel
    {
        public List<SubscriptionModel> Subscriptions { get; set; }
        public int AccountSum { get; set; }
    }
}
