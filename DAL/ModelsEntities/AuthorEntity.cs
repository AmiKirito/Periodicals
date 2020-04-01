using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.ModelsEntities
{
    /// <summary>
    /// Class that represents author entity for data access layer
    /// </summary>
    [Table("Authors")]
    public class AuthorEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<PublisherEntity> Publishers { get; set; }
    }
}
