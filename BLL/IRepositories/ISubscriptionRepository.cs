using BLL.Models;
using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface ISubscriptionRepository
    {
        List<Subscription> GetAll();
    }
}
