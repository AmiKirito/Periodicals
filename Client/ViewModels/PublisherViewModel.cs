using BLL.Models;
using Client.Models;
using System.Collections.Generic;

namespace Client.ViewModels
{
    public class PublisherViewModel
    {
        public IEnumerable<Publisher> Publishers { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}