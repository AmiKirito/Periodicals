using System.Collections.Generic;

namespace BLL.Models
{
    /// <summary>
    /// Class that represents the author model for business logic and presentation layers
    /// </summary>
    public class Author
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Publisher> Publishers { get; set; }
        public Author() { }
        public Author(string id, string name)
        {
            Id = id;
            Name = name;
            Publishers = new List<Publisher>();
        }
    }
}
