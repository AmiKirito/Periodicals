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
        string AddPublisher(Publisher publisher);
        string UpdatePublisher(Publisher publisher);
        void Remove(string publisherId);
    }
}
