using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace DAL.ModelsEntities
{
    public class UserEntity : IdentityUser
    {
        public List<SubscriptionEntity> Subscriptions { get; set; }
        public int AccountSum { get; set; }
    }
}
