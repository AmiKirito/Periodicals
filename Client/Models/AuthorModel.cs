using System.Collections.Generic;

namespace Client.Models
{
    public class AuthorModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PublisherModel> Publishers { get; set; }
    }
}
