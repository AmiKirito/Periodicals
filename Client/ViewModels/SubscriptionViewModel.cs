using BLL.Models;
using Client.Models;
using System.Collections.Generic;

namespace Client.ViewModels
{
    public class SubscriptionViewModel
    {
        public IEnumerable<Subscription> Subscriptions { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}