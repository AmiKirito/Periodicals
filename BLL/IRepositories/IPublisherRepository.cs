using BLL.Models;
using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface IPublisherRepository
    {
        List<Publisher> GetAllPublishers();
        int CountAllPublishers();
    }
}
