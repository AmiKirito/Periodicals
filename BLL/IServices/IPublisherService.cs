using BLL.Models;
using System.Collections.Generic;

namespace BLL.IServices
{
    public interface IPublisherService
    {
        List<Publisher> GetAll();
        int CountAll();
    }
}
