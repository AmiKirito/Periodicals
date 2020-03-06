using System.Collections.Generic;

namespace Client.Models
{
    public class TopicModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<PublisherModel> Publishers { get; set; }
    }
}
