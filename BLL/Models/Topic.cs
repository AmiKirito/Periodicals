using System.Collections.Generic;

namespace BLL.Models
{
    public class Topic
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Publisher> Publishers { get; set; }
        public Topic(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
