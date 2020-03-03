using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Topic
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Publisher> Publishers { get; set; }
    }
}
