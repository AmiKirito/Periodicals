using BLL.Models;
using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface IPublisherRepository
    {
        List<Publisher> GetAll();
        int CountAll();
        Publisher GetById(string id);
        /// <summary>
        /// Method that gets all authors for further manipulations with publishers
        /// </summary>
        List<Author> GetAuthors();
        /// <summary>
        /// Method that gets all topics for further manipulations with publishers
        /// </summary>
        List<Topic> GetTopics();
        string AddPublisher(Publisher publisher);
        string UpdatePublisher(Publisher publisher);
        void Remove(string publisherId);
    }
}
