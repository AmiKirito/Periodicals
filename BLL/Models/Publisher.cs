using System.Collections.Generic;

namespace BLL.Models
{
    public class Publisher
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MonthlySubscriptionPrice { get; set; }
        public string ImagePath { get; set; }
        public List<Author> Authors { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Subscription> Subscriptions { get; set; }
        public bool IsRemoved { get; set; }
        public Publisher() { }
        public Publisher(string id, string title, string description, int monthlyPrice, string imagePath, bool isRemoved)
        {
            Id = id;
            Title = title;
            Description = description;
            MonthlySubscriptionPrice = monthlyPrice;
            IsRemoved = isRemoved;
            ImagePath = imagePath;
            Authors = new List<Author>();
            Topics = new List<Topic>();
            Subscriptions = new List<Subscription>();
        }
    }
}
