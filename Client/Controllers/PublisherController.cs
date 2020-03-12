using BLL.IServices;
using Client.Models;
using Client.ViewModels;
using System.Collections.Generic;
using BLL.Models;
using System.Linq;
using System.Web.Mvc;
using System;
using Microsoft.AspNet.Identity;

namespace Periodicals.Controllers
{
    public class PublisherController : Controller
    {
        private readonly IPublisherService _publisherService;
        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }
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
        public ActionResult Details(int? id)
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

            return RedirectToAction("Details", "Publisher",  new { id = 1 });
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