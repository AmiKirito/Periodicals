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

        public int CountAll()
        {
            return _publisherRepository.CountAllPublishers();
        }

        public List<Publisher> GetAll()
        {
            return _publisherRepository.GetAllPublishers();
        }
    }
}
