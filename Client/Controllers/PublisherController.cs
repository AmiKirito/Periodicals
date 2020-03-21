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
        private string[] validImageTypes = new string[]
        {
        "image/jpg",
        "image/jpeg",
        "image/png"
        };

        public PublisherController(IPublisherService publisherService, ILogger logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(int page = 1, string search = "", bool sortTitle = false, bool sortPrice = false, string filterTopic = "")
        {
            _logger.Information("Action: Index; Controller: Publisher; Call method: GET;");
            try
            {
                ViewBag.SortTitle = sortTitle == false ? false : true;
                ViewBag.SortPrice = sortPrice == false ? false : true;
                ViewBag.Search = search;
                ViewBag.Filter = filterTopic;

                int pageSize = 8;

                IEnumerable<Publisher> publishersPerPage = _publisherService.GetPublishers();

                if (!string.IsNullOrEmpty(search))
                {
                    publishersPerPage = publishersPerPage.Where(p => p.Title.ToLower().Contains(search.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(filterTopic))
                {
                    publishersPerPage = publishersPerPage.Where(p => p.Topics.Any(t => t.Title.Contains(filterTopic))).ToList();
                }
                if (sortTitle)
                {
                    publishersPerPage = publishersPerPage.OrderBy(p => p.Title).ToList();
                }
                if (sortPrice)
                {
                    publishersPerPage = publishersPerPage.OrderByDescending(p => p.MonthlySubscriptionPrice).ToList();
                }
                if (sortPrice && sortTitle)
                {
                    publishersPerPage = publishersPerPage.OrderBy(p => p.Title).ThenByDescending(p => p.MonthlySubscriptionPrice).ToList();
                }

                var totalItems = publishersPerPage.Count();

                if (page > Math.Ceiling(((double)totalItems / (double)pageSize)) && totalItems != 0)
                {
                    return View("NotFound");
                }

                publishersPerPage = publishersPerPage.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

                var existingTopics = ConvertToSelectListTopics(_publisherService.GetExistingTopics());

                PublisherViewModel model = new PublisherViewModel { Publishers = publishersPerPage, PageInfo = pageInfo, ExistingTopics = existingTopics };

                return View(model);
            } catch(Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            _logger.Information("Action: Details; Controller: Publisher; Call method: GET;");
            try
            {
                if (id == null)
                {
                    return View("NotFound");
                }

                var publisherModel = _publisherService.GetPublisherById(id.ToString());

                if(publisherModel.IsRemoved == true)
                {
                    return View("NotFound");
                }

                var userId = User.Identity.GetUserId();

                var model = new PublisherDetailsViewModel
                {
                    Publisher = publisherModel,
                    UserId = userId
                };

                return View(model);
            } catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpGet]
        public ActionResult New()
        {
            _logger.Information("Action: New; Controller: Publisher; Call method: GET;");
            try
            {
                PublisherCreateViewModel model = new PublisherCreateViewModel();

                var allAuthors = _publisherService.GetExistingAuthors();
                var allTopics = _publisherService.GetExistingTopics();

                model.ExistingAuthors = ConvertToSelectListAuthors(allAuthors);
                model.ExistingTopics = ConvertToSelectListTopics(allTopics);

                return View(model);
            }
            catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpPost]
        public ActionResult New(PublisherCreateViewModel model)
        {
            _logger.Information("Action: New; Controller: Publisher; Call method: POST;");
            try
            {

                if (!ModelState.IsValid)
                {
                    model.ExistingAuthors = ConvertToSelectListAuthors(_publisherService.GetExistingAuthors());
                    model.ExistingTopics = ConvertToSelectListTopics(_publisherService.GetExistingTopics());

                    return View(model);
                }                

                if (model.Image == null || model.Image.ContentLength == 0)
                {
                    model.ExistingAuthors = ConvertToSelectListAuthors(_publisherService.GetExistingAuthors());
                    model.ExistingTopics = ConvertToSelectListTopics(_publisherService.GetExistingTopics());

                    ModelState.AddModelError("", "This field is required");
                    return View(model);
                }
                if (!validImageTypes.Any(model.Image.ContentType.Contains))
                {
                    model.ExistingAuthors = ConvertToSelectListAuthors(_publisherService.GetExistingAuthors());
                    model.ExistingTopics = ConvertToSelectListTopics(_publisherService.GetExistingTopics());

                    ModelState.AddModelError("", "Please choose either jpg, jpeg or png format file");
                    return View(model);
                }

                Publisher publisherToAdd = new Publisher();

                publisherToAdd.Title = model.Title;
                publisherToAdd.Description = model.Description;
                publisherToAdd.MonthlySubscriptionPrice = model.MonthlySubscriptionPrice;

                publisherToAdd.ImagePath = GenerateImageSavePath(model.Image);
                publisherToAdd.Authors = ConvertToListAuthors(model.Authors);
                publisherToAdd.Topics = ConvertToListTopics(model.Topics);

                var newPublisherId = _publisherService.AddNewPublisher(publisherToAdd);

                return RedirectToAction("Details", "Publisher", new { id = newPublisherId });
            } catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            _logger.Information("Action: Edit; Controller: Publisher; Call method: GET;");
            try
            {
                if (id == null)
                {
                    return View("NotFound");
                }

                PublisherEditViewModel publisherToEdit = new PublisherEditViewModel();
                var publisher = _publisherService.GetPublisherById(id.ToString());

                publisherToEdit.Id = publisher.Id;
                publisherToEdit.IsRemoved = publisher.IsRemoved;
                publisherToEdit.Title = publisher.Title;
                publisherToEdit.Description = publisher.Description;
                publisherToEdit.ExistingImagePath = publisher.ImagePath;
                publisherToEdit.MonthlySubscriptionPrice = publisher.MonthlySubscriptionPrice;

                SetPublisherToEditTopics(publisherToEdit, publisher);
                SetPublisherToEditAuthors(publisherToEdit, publisher);

                return View(publisherToEdit);
            }
            catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }   
        }
        [HttpPost]
        public ActionResult Edit(PublisherEditViewModel model)
        {
            _logger.Information("Action: Edit; Controller: Publisher; Call method: POST;");
            try 
            {
                var publisher = _publisherService.GetPublisherById(model.Id.ToString());

                if (!ModelState.IsValid)
                {
                    SetPublisherToEditAuthors(model, publisher);
                    SetPublisherToEditTopics(model, publisher);
                    return View(model);
                }

                Publisher publisherToChanage = new Publisher
                {
                    Id = model.Id,
                    IsRemoved = model.IsRemoved,
                    Title = model.Title,
                    Description = model.Description,
                    MonthlySubscriptionPrice = model.MonthlySubscriptionPrice
                };

                if (model.Image != null)
                {
                    publisherToChanage.ImagePath = GenerateImageSavePath(model.Image);

                    if (!validImageTypes.Any(model.Image.ContentType.Contains))
                    {
                        SetPublisherToEditAuthors(model, publisher);
                        SetPublisherToEditTopics(model, publisher);

                        ModelState.AddModelError("", "Please choose either jpg, jpeg or png format file");
                        return View(model);
                    }   
                }
                else
                {
                    publisherToChanage.ImagePath = model.ExistingImagePath;
                }

                publisherToChanage.Authors = ConvertToListAuthors(model.Authors);
                publisherToChanage.Topics = ConvertToListTopics(model.Topics);

                string publisherToChangeId = _publisherService.UpdatePublisher(publisherToChanage);

                return RedirectToAction("Details", "Publisher", new { id = publisherToChangeId });
            } catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
            
        }
        public ActionResult Remove(int? id)
        {
            _logger.Information("Action: Remove; Controller: Publisher; Call method: POST;");
            try 
            {
                if (id == null)
                {
                    return View("NotFound");
                }

                _publisherService.RemovePublisher(id.ToString());

                return RedirectToAction("Index", "Publisher");
            } catch (Exception e) {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        private void SetPublisherToEditTopics(PublisherEditViewModel publisherToEdit, Publisher publisher)
        {
            var publisherTopicsArray = publisher.Topics.ToArray();
            var existingTopicsList = _publisherService.GetExistingTopics();

            var publisherToEditExistingTopicsList = new List<Topic>();

            for (int i = 0; i < publisherTopicsArray.Length; i++)
            {
                if (i == publisher.Topics.ToArray().Length - 1)
                {
                    publisherToEdit.Topics += publisherTopicsArray[i].Title;
                    break;
                }

                publisherToEdit.Topics += publisherTopicsArray[i].Title + ";";
            }

            foreach (var existingTopic in existingTopicsList)
            {
                if(!publisher.Topics.Any(t => t.Title == existingTopic.Title))
                {
                    publisherToEditExistingTopicsList.Add(existingTopic);
                }
            }

            publisherToEdit.ExistingTopics = ConvertToSelectListTopics(publisherToEditExistingTopicsList);
        }
        private void SetPublisherToEditAuthors(PublisherEditViewModel publisherToEdit, Publisher publisher)
        {
            var publisherAuthorsArray = publisher.Authors.ToArray();
            var existingAuthorsList = _publisherService.GetExistingAuthors();

            var publisherToEditExistingAuthorsList = new List<Author>();

            for (int i = 0; i < publisherAuthorsArray.Length; i++)
            {
                if (i == publisher.Authors.ToArray().Length - 1)
                {
                    publisherToEdit.Authors += publisherAuthorsArray[i].Name;
                    break;
                }

                publisherToEdit.Authors += publisherAuthorsArray[i].Name + ";";
            }

            foreach (var existingAuthor in existingAuthorsList)
            {
                if(!publisher.Authors.Any(a => a.Name == existingAuthor.Name))
                {
                    publisherToEditExistingAuthorsList.Add(existingAuthor);
                }
            }

            publisherToEdit.ExistingAuthors = ConvertToSelectListAuthors(publisherToEditExistingAuthorsList);
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
                if(authorString != " " && authorString !="" && !string.IsNullOrEmpty(authorString))
                {
                    var author = new Author
                    {
                        Name = authorString
                    };

                    authorsList.Add(author);
                }
            }

            return authorsList;
        }
        private List<Topic> ConvertToListTopics(string topicsString)
        {
            var topicsList = new List<Topic>();
            var topicsArray = topicsString.Split(';');

            foreach (var topicString in topicsArray)
            {
                if (topicString != " " && !string.IsNullOrEmpty(topicString))
                {
                    var topic = new Topic
                    {
                        Title = topicString
                    };

                    topicsList.Add(topic);
                }
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
        private void LogException(Exception e)
        {
            var exceptionType = e.GetType().Name;
            var exceptionMessage = e.Message;
            var stackTrace = e.StackTrace.ToString();
            var source = e.Source.ToString();

            _logger.Information($"Exception: Type - {exceptionType}; Message - {exceptionMessage};" +
                $" StackTrace - {stackTrace};" +
                $" Source - {source};");
        }
    }
}