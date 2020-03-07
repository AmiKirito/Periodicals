using BLL.Models;
using System.Collections.Generic;

namespace BLL.IServices
{
    public interface IPublisherService
    {
        List<Publisher> GetPublishers();
        int CountPublishers();
    }
}
