﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.ModelsEntities
{
    /// <summary>
    /// Class that represents publisher entity for data access layer
    /// </summary>
    [Table("Publishers")]
    public class PublisherEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MonthlySubscriptionPrice { get; set; }
        public string ImagePath { get; set; }
        public List<AuthorEntity> Authors { get; set; }
        public List<TopicEntity> Topics { get; set; }
        public List<SubscriptionEntity> Subscriptions { get; set; }
        public bool IsRemoved { get; set; }
        public PublisherEntity()
        {
            Authors = new List<AuthorEntity>();
            Topics = new List<TopicEntity>();
            Subscriptions = new List<SubscriptionEntity>();
        }
    }
}
