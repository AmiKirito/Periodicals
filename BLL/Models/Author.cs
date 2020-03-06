using System.Collections.Generic;

namespace BLL.Models
{
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Publisher> Publishers { get; set; }
        public Author(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
