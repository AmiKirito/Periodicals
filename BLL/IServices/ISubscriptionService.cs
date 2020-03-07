using BLL.Models;
using System.Collections.Generic;

namespace BLL.IServices
{
    public interface ISubscriptionService
    {
        List<Subscription> GetSubscriptions();
        
    }
}
