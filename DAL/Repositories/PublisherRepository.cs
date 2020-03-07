using BLL.IRepositories;
using BLL.Models;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly AppDbContext _context;
        public PublisherRepository(AppDbContext context)
        {
            _context = context;
        }

        public int CountAll()
        {
            var publishersCount = _context.Publishers.Count();
            return publishersCount;
        }

        public List<Publisher> GetAll()
        {
            var publishersQuerry = _context.Publishers.Include("Authors").Include("Topics").Include("Subscriptions").ToList();
            var publishers = new List<Publisher>();
            foreach (var publisherEntity in publishersQuerry)
            {
                var authors = new List<Author>();

                foreach (var authorEntity in publisherEntity.Authors)
                {
                    var author = new Author(authorEntity.Id, authorEntity.Name);

                    authors.Add(author);
                }

                var topics = new List<Topic>();

                foreach (var topicEntity in publisherEntity.Topics)
                {
                    var topic = new Topic(topicEntity.Id, topicEntity.Title);

                    topics.Add(topic);
                }

                var subsriptions = new List<Subscription>();

                foreach (var subscriptionEntity in publisherEntity.Subscriptions)
                {
                    var subscription = new Subscription(subscriptionEntity.Id, subscriptionEntity.Price,
                                                        subscriptionEntity.ExpirationDate.Date, subscriptionEntity.PublisherId,
                                                        subscriptionEntity.SubscriptionPeriod, subscriptionEntity.UserId);

                    subsriptions.Add(subscription);
                }

                var publisher = new Publisher(publisherEntity.Id, publisherEntity.Title, publisherEntity.Description);

                authors.ForEach(author => publisher.Authors.Add(author));
                topics.ForEach(topic => publisher.Topics.Add(topic));
                subsriptions.ForEach(subsription => publisher.Subscriptions.Add(subsription));

                publishers.Add(publisher);
            }
            return publishers;
        }
    }
}
