using BLL.Models;
using Client.Models;
using System.Collections.Generic;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents subscription ViewModel for business logic and presentation layers
    /// </summary>
    public class SubscriptionViewModel
    {
        public IEnumerable<Subscription> Subscriptions { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}