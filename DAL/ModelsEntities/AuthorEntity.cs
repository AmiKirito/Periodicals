using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.ModelsEntities
{
    [Table("Authors")]
    public class AuthorEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PublisherEntity> Publishers { get; set; }
    }
}
