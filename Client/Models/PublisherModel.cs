using System.Collections.Generic;

namespace Client.Models
{
    public class PublisherModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<AuthorModel> Authors { get; set; }
        public List<TopicModel> Topics { get; set; }
        public List<SubscriptionModel> Subscriptions { get; set; }
    }
}
