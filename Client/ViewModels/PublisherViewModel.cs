using BLL.Models;
using Client.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents publisher ViewModel for business logic and presentation layers
    /// </summary>
    public class PublisherViewModel
    {
        public IEnumerable<Publisher> Publishers { get; set; }
        public PageInfo PageInfo { get; set; }
        public List<SelectListItem> ExistingTopics { get; set; }
    }
}