using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.ModelsEntities
{
    [Table("Publishers")]
    public class PublisherEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<AuthorEntity> Authors { get; set; }
        public List<TopicEntity> Topics { get; set; }
        public List<SubscriptionEntity> Subscriptions { get; set; }
        public bool IsRemoved { get; set; }
    }
}
