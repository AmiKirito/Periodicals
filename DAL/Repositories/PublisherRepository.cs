using BLL.IRepositories;
using BLL.Models;
using DAL.ModelsEntities;
using System;
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
            var publishersCount = _context.Publishers.Where(p => p.IsRemoved == false).Count();
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
                Id = (Convert.ToInt32(_context.Publishers.ToList()
                        .OrderByDescending(p => Convert.ToInt32(p.Id))
                        .ToList().First().Id) + 1).ToString(),
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
                        Id = (Convert.ToInt32(_context.Topics.ToList()
                                .OrderByDescending(t => Convert.ToInt32(t.Id))
                                .ToList().First().Id) + 1).ToString(),
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
                        Id = (Convert.ToInt32(_context.Authors.ToList()
                            .OrderByDescending(a => Convert.ToInt32(a.Id))
                            .ToList().First().Id) + 1).ToString(),
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

        public string UpdatePublisher(Publisher publisherToChange)
        {
            var publisherEntity = _context.Publishers.Include("Authors").Include("Topics").Include("Subscriptions").Where(p => p.Id == publisherToChange.Id).First();

            publisherEntity.Title = publisherToChange.Title;
            publisherEntity.Description = publisherToChange.Title;
            publisherEntity.MonthlySubscriptionPrice = publisherToChange.MonthlySubscriptionPrice;
            publisherEntity.ImagePath = publisherToChange.ImagePath;
            publisherEntity.IsRemoved = publisherToChange.IsRemoved;

            publisherEntity.Authors = new List<AuthorEntity>();
            _context.SaveChanges();

            foreach (var author in publisherToChange.Authors)
            {
                var authorEntity = new AuthorEntity
                {
                    Name = author.Name
                };

                if (!_context.Authors.Any(a => a.Name == author.Name))
                {
                    authorEntity.Id = (Convert.ToInt32(_context.Authors.ToList()
                                        .OrderByDescending(a => Convert.ToInt32(a.Id))
                                        .ToList().First().Id) + 1).ToString();

                    _context.Authors.Add(authorEntity);
                    publisherEntity.Authors.Add(authorEntity);
                }
                else
                {
                    authorEntity = _context.Authors.Where(a => a.Name == author.Name).First();
                    publisherEntity.Authors.Add(authorEntity);
                }

                _context.SaveChanges();
            }

            publisherEntity.Topics = new List<TopicEntity>();
            _context.SaveChanges();

            foreach (var topic in publisherToChange.Topics)
            {
                var topicEntity = new TopicEntity
                {
                    Title = topic.Title
                };

                if (!_context.Topics.Any(t => t.Title == topic.Title))
                {
                    topicEntity.Id = (Convert.ToInt32(_context.Topics.ToList()
                                    .OrderByDescending(t => Convert.ToInt32(t.Id))
                                    .ToList().First().Id) + 1).ToString();

                    _context.Topics.Add(topicEntity);
                    publisherEntity.Topics.Add(topicEntity);
                }
                else
                {
                    topicEntity = _context.Topics.Where(t => t.Title == topic.Title).First();
                    publisherEntity.Topics.Add(topicEntity);
                }

                _context.SaveChanges();               
            }

            _context.SaveChanges();

            return publisherToChange.Id;
        }

        public void Remove(string publisherId)
        {
            var publisherToRemove = _context.Publishers.Where(p => p.Id == publisherId).First();

            publisherToRemove.IsRemoved = true;

            _context.SaveChanges();
        }
    }
}
