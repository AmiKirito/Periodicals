using BLL.IServices;
using BLL.Models;
using System.Collections.Generic;
using BLL.IRepositories;

namespace BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository _publisherRepository;
        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public string AddNewPublisher(Publisher publisher)
        {
            return _publisherRepository.AddPublisher(publisher);
        }

        public int CountPublishers()
        {
            return _publisherRepository.CountAll();
        }

        public List<Author> GetExistingAuthors()
        {
            return _publisherRepository.GetAuthors();
        }

        public List<Topic> GetExistingTopics()
        {
            return _publisherRepository.GetTopics();
        }

        public Publisher GetPublisherById(string publisherId)
        {
            return _publisherRepository.GetById(publisherId);
        }

        public List<Publisher> GetPublishers()
        {
            return _publisherRepository.GetAll();
        }
    }
}
