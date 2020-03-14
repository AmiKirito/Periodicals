using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Client.ViewModels
{
    public class PublisherCreateViewModel
    {
        [Required(ErrorMessage = "The title field is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The description field is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The monthly price field is required")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "Please enter valid sum")]
        public int MonthlySubscriptionPrice { get; set; }
        [Required(ErrorMessage = "The image field is required")]
        public HttpPostedFileBase Image { get; set; }
        [Required(ErrorMessage = "The authors field is required")]
        public string Authors { get; set; }
        [Required(ErrorMessage = "The topics field is required")]   
        public string Topics { get; set; }
        public List<SelectListItem> ExistingAuthors;
        public List<SelectListItem> ExistingTopics;
        public PublisherCreateViewModel()
        {
            ExistingAuthors = new List<SelectListItem>();
            ExistingTopics = new List<SelectListItem>();
        }
    }
}