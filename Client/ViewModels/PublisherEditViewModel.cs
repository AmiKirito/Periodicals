using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents editing publisher ViewModel for business logic and presentation layers
    /// </summary>
    public class PublisherEditViewModel
    {
        public string Id { get; set; }
        public bool IsRemoved { get; set; }
        [Required(ErrorMessage = "The title field is required")]
        [Display(Name = "Title:")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The description field is required")]
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The monthly price field is required")]
        [RegularExpression("[1-9][0-9]*", ErrorMessage = "Please enter valid sum")]
        [Display(Name = "Price/month:")]
        public int MonthlySubscriptionPrice { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ExistingImagePath { get; set; }
        [Required(ErrorMessage = "The authors field is required")]
        [Display(Name = "Authors:")]
        public string Authors { get; set; }
        [Required(ErrorMessage = "The topics field is required")]
        [Display(Name = "Topics")]
        public string Topics { get; set; }
        public List<SelectListItem> ExistingAuthors;
        public List<SelectListItem> ExistingTopics;
        public PublisherEditViewModel()
        {
            ExistingAuthors = new List<SelectListItem>();
            ExistingTopics = new List<SelectListItem>();
        }
    }
}