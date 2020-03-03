using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Publisher
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public List<Author> Authors { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}
