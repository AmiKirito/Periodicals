using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.ModelsEntities
{
    [Table("Topics")]
    public class TopicEntity
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<PublisherEntity> Publishers { get; set; }
    }
}
