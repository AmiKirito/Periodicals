using System.Collections.Generic;

namespace BLL.Models
{
    public class User
    {
        public List<Subscription> Subscriptions { get; set; }
        public int AccountSum { get; set; }
    }
}
