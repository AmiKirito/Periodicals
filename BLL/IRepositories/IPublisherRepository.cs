using BLL.Models;
using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface IPublisherRepository
    {
        List<Publisher> GetAll();
        int CountAll();
        Publisher GetById(string id);
        List<Author> GetAuthors();
        List<Topic> GetTopics();
    }
}
