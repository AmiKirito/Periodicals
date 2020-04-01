using System.Collections.Generic;

namespace BLL.Models
{
    /// <summary>
    /// Class that represents the user model for business logic and presentation layers
    /// </summary>
    public class User
    {
        public List<Subscription> Subscriptions { get; set; }
        public int AccountSum { get; set; }
    }
}
