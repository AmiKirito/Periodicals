using BLL.Models;
using System.Collections.Generic;

namespace BLL.IServices
{
    public interface IPublisherService
    {
        List<Publisher> GetPublishers();
        int CountPublishers();
        Publisher GetPublisherById(string id);
        List<Author> GetExistingAuthors();
        List<Topic> GetExistingTopics();
        string AddNewPublisher(Publisher publisher);
        string UpdatePublisher(Publisher publisher);
        void RemovePublisher(string publisherId);
    }
}
