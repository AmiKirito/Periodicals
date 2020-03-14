using BLL.IServices;
using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using BLL.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using Microsoft.AspNet.Identity;
using Serilog;
using System.Web;
using System.IO;

namespace Periodicals.Controllers
{
    [Authorize(Roles = "Moderator, Admin, SuperAdmin")]
    public class PublisherController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger _logger;


        public PublisherController(IPublisherService publisherService, ILogger logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }
        [AllowAnonymous]
        public ActionResult Index(int page = 1)
        {
            int pageSize = 8;
            int totalItems = _publisherService.CountPublishers();

            if (page > Math.Ceiling(((double)totalItems / (double)pageSize)))
            {
                return View("NotFound");
            }
            IEnumerable<Publisher> publishersPerPage = _publisherService.GetPublishers().Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

            PublisherViewModel model = new PublisherViewModel { Publishers = publishersPerPage, PageInfo = pageInfo };

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    id = 1;
                }

                var publisherModel = _publisherService.GetPublisherById(id.ToString());
                var userId = User.Identity.GetUserId();

                var model = new PublisherDetailsViewModel
                {
                    Publisher = publisherModel,
                    UserId = userId
                };

                return View(model);
            } catch (Exception e) {
                var exceptionType = e.GetType().Name;
                var exceptionMessage = e.Message;
                return RedirectToAction("Index", "Error", new { exceptionType, exceptionMessage });
            }
        }
        [HttpGet]
        public ActionResult New()
        {
            PublisherCreateViewModel model = new PublisherCreateViewModel();

            var allAuthors = _publisherService.GetExistingAuthors();
            var allTopics = _publisherService.GetExistingTopics();

            model.ExistingAuthors = ConvertToSelectListAuthors(allAuthors);
            model.ExistingTopics = ConvertToSelectListTopics(allTopics);

            return View(model);
        }
        [HttpPost]
        public ActionResult New(PublisherCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var validImageTypes = new string[]
            {
                "image/jpg",
                "image/jpeg",
                "image/png"
            };

            if (model.Image == null || model.Image.ContentLength == 0)
            {
                ModelState.AddModelError("ImageUpload", "This field is required");
            }
            if(!validImageTypes.Any(model.Image.ContentType.Contains))
            {
                ModelState.AddModelError("ImageUpload", "Please choose either jpg, jpeg or png format file");
            }

            Publisher publisherToAdd = new Publisher();

            publisherToAdd.Title = model.Title;
            publisherToAdd.Description = model.Description;
            publisherToAdd.MonthlySubscriptionPrice = model.MonthlySubscriptionPrice;

            publisherToAdd.ImagePath = GenerateImageSavePath(model.Image);
            publisherToAdd.Authors = ConvertToListAuthors(model.Authors);
            publisherToAdd.Topics = ConvertToListTopics(model.Topics);

            var newPublisherId = _publisherService.AddNewPublisher(publisherToAdd);

            return RedirectToAction("Details", "Publisher",  new { id = newPublisherId });
        }
        private string GenerateImageSavePath(HttpPostedFileBase image)
        {
            string uniqueImageLocation = "/Content/images/publisherImages";
            var fileName = Path.GetFileName(image.FileName);
            var guidFileName = $"{Guid.NewGuid()}_{fileName}";
            uniqueImageLocation = uniqueImageLocation + "/" + guidFileName;

            var imagesFilePath = "~/Content/images/publisherImages";
            var saveFilePath = Path.Combine(imagesFilePath, guidFileName);

            image.SaveAs(Server.MapPath(saveFilePath));

            return uniqueImageLocation;
        }
        private List<Author> ConvertToListAuthors(string authorsString)
        {
            var authorsList = new List<Author>();
            var authorsArray = authorsString.Split(';');

            foreach (var authorString in authorsArray)
            {
                var author = new Author
                {
                    Name = authorString
                };

                authorsList.Add(author);
            }

            return authorsList;
        }
        private List<Topic> ConvertToListTopics(string topicsString)
        {
            var topicsList = new List<Topic>();
            var topicsArray = topicsString.Split(';');

            foreach (var topicString in topicsArray)
            {
                var topic = new Topic
                {
                    Title = topicString
                };

                topicsList.Add(topic);
            }

            return topicsList;
        }
        private List<SelectListItem> ConvertToSelectListAuthors(List<Author> authors)
        {
            var authorsSelectList = new List<SelectListItem>();

            SelectListItem selected = new SelectListItem
            {
                Text = "Select author",
                Selected = true,
                Value = ""
            };

            authorsSelectList.Add(selected);

            foreach (var author in authors)
            {
                var authorSelectItem = new SelectListItem
                {
                    Text = author.Name,
                    Value = author.Name
                };

                authorsSelectList.Add(authorSelectItem);
            }

            return authorsSelectList;
        }
        private List<SelectListItem> ConvertToSelectListTopics(List<Topic> topics)
        {
            var topicsSelectList = new List<SelectListItem>();

            SelectListItem selected = new SelectListItem
            {
                Text = "Select topic",
                Selected = true,
                Value = ""
            };

            topicsSelectList.Add(selected);

            foreach (var topic in topics)
            {
                var topicSelectItem = new SelectListItem
                {
                    Text = topic.Title,
                    Value = topic.Title
                };

                topicsSelectList.Add(topicSelectItem);
            }

            return topicsSelectList;
        }
    }
}