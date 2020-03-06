using System.Collections.Generic;

namespace BLL.Models
{
    public class Publisher
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public Publisher(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
