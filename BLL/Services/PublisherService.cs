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

        public int CountPublishers()
        {
            return _publisherRepository.CountAll();
        }

        public List<Publisher> GetPublishers()
        {
            return _publisherRepository.GetAll();
        }
    }
}
