using BLL.IRepositories;
using BLL.Models;
using DAL.ModelsEntities;
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
            var publishersQuerry = _context.Publishers.Include("Authors").Include("Topics").Include("Subscriptions").Where(p => p.IsRemoved == false).ToList();
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
                                                        subscriptionEntity.SubscriptionPeriod, subscriptionEntity.UserId, subscriptionEntity.IsRemoved,
                                                        subscriptionEntity.IsExpired);
                    subsriptions.Add(subscription);
                }

                var publisher = new Publisher(publisherEntity.Id, publisherEntity.Title, publisherEntity.Description,
                                              publisherEntity.MonthlySubscriptionPrice, publisherEntity.ImagePath, publisherEntity.IsRemoved);

                authors.ForEach(author => publisher.Authors.Add(author));
                topics.ForEach(topic => publisher.Topics.Add(topic));
                subsriptions.ForEach(subsription => publisher.Subscriptions.Add(subsription));

                publishers.Add(publisher);
            }
            return publishers;
        }

        public Publisher GetById(string publisherId)
        {
            var publisherEntity = _context.Publishers.Include("Authors").Include("Topics").Include("Subscriptions").Where(p => p.Id == publisherId).First();

            Publisher publisher = new Publisher
            {
                Id = publisherEntity.Id,
                Title = publisherEntity.Title,
                Description = publisherEntity.Description,
                ImagePath = publisherEntity.ImagePath,
                MonthlySubscriptionPrice = publisherEntity.MonthlySubscriptionPrice,
                IsRemoved = publisherEntity.IsRemoved,
                Authors = new List<Author>(),
                Topics = new List<Topic>(),
                Subscriptions = new List<Subscription>()
            };

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
                                                    subscriptionEntity.ExpirationDate, subscriptionEntity.PublisherId,
                                                    subscriptionEntity.SubscriptionPeriod, subscriptionEntity.UserId,
                                                    subscriptionEntity.IsRemoved, subscriptionEntity.IsExpired);
                subsriptions.Add(subscription);
            }

            authors.ForEach(author => publisher.Authors.Add(author));
            topics.ForEach(topic => publisher.Topics.Add(topic));
            subsriptions.ForEach(subsription => publisher.Subscriptions.Add(subsription));

            return publisher;
        }
        public List<Author> GetAuthors()
        {
            var authorsQuery = _context.Authors.ToList();
            List<Author> authors = new List<Author>();
            
            foreach (var authorEntity in authorsQuery)
            {
                var author = new Author
                {
                    Id = authorEntity.Id,
                    Name = authorEntity.Name
                };

                authors.Add(author);
            }

            return authors;
        }
        public List<Topic> GetTopics()
        {
            var topicsQuery = _context.Topics.ToList();
            List<Topic> topics = new List<Topic>();

            foreach (var topicEntity in topicsQuery )
            {
                var topic = new Topic
                {
                    Id = topicEntity.Id,
                    Title = topicEntity.Title
                };

                topics.Add(topic);
            }

            return topics;
        }
        public string AddPublisher(Publisher publisher)
        {
            var existingTopics = _context.Topics.ToList();
            var existingAuthors = _context.Authors.ToList();
            var publisherEntity = new PublisherEntity
            {
                Id = (_context.Publishers.Count() + 1).ToString(),
                Title = publisher.Title,
                Description = publisher.Description,
                MonthlySubscriptionPrice = publisher.MonthlySubscriptionPrice,
                ImagePath = publisher.ImagePath,
                IsRemoved = false
            };

            foreach (var topic in publisher.Topics)
            {
                if(existingTopics.Any(t => t.Title == topic.Title))
                {
                    publisherEntity.Topics.Add(existingTopics.Where(t => t.Title == topic.Title).First());
                }
                else
                {
                    var topicEntity = new TopicEntity
                    {
                        Id = (_context.Topics.Count() + 1).ToString(),
                        Title = topic.Title
                    };

                    publisherEntity.Topics.Add(topicEntity);

                    _context.Topics.Add(topicEntity);
                }
                _context.SaveChanges();
            }
            foreach (var author in publisher.Authors)
            {
                if(existingAuthors.Any(a => a.Name == author.Name))
                {
                    publisherEntity.Authors.Add(existingAuthors.Where(a => a.Name == author.Name).First());
                }
                else
                {
                    var authorEntity = new AuthorEntity
                    {
                        Id = (_context.Authors.Count() + 1).ToString(),
                        Name = author.Name
                    };

                    publisherEntity.Authors.Add(authorEntity);

                    _context.Authors.Add(authorEntity);
                }
                _context.SaveChanges();
            }

            _context.Publishers.Add(publisherEntity);
            _context.SaveChanges();

            return publisherEntity.Id;
        }
    }
}
