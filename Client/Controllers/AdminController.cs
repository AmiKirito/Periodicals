using Client.ViewModels;
using DAL.ModelsEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Client.App_Start;
using System.Web;
using System.Web.Mvc;
using BLL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using Client.Models;
using Serilog;

namespace Periodicals.Controllers
{
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger _logger;

        public AdminController(IAdminService adminService, ILogger logger)
        {
            _adminService = adminService;
            _logger = logger;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public ApplicationRoleManager RoleManager
        { 
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        public ActionResult Users(int page = 1)
        {
            _logger.Information("Action: Users; Controller: Admin; Call method: GET;");
            try
            {
                int pageSize = 2;
                var userIds = _adminService.GetUserIdList();
                int totalItems = userIds.Count();

                if (page > Math.Ceiling(((double)totalItems / (double)pageSize)) && totalItems != 0)
                {
                    return View("NotFound");
                }

                List<Tuple<string, string, string, bool, bool, string>> preModel = new List<Tuple<string, string, string, bool, bool, string>>();
                PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

                UsersViewModel model = new UsersViewModel();

                foreach (var userId in userIds)
                {
                    var user = UserManager.FindById(userId);
                    bool isLocked = false;
                    string username = UserManager.FindById(userId).UserName;
                    string role = UserManager.GetRoles(userId).FirstOrDefault();

                    if (user.LockoutEndDateUtc > DateTime.Now && user.LockoutEndDateUtc != null)
                    {
                        isLocked = true;
                    }

                    bool canLock = HasRights(userId);

                    preModel.Add(new Tuple<string, string, string, bool, bool, string>(userId, username, role, isLocked, canLock, userId));
                }

                model.Items = preModel;
                model.Items = model.Items.Skip((page - 1) * pageSize).Take(pageSize);
                model.PageInfo = pageInfo;

                return View(model);
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            _logger.Information("Action: CreateUser; Controller: Admin; Call method: GET;");
            try
            {
                CreateUserViewModel model = new CreateUserViewModel();

                model.ExistingRoles = GetRolesForUser();

                return View(model);
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        [HttpPost]
        public ActionResult CreateUser(CreateUserViewModel model)
        {
            _logger.Information("Action: Crea   teUser; Controller: Admin; Call method: POST;");
            try
            {
                if (!ModelState.IsValid)
                {
                    model.ExistingRoles = GetRolesForUser();
                    ModelState.AddModelError("", "Invalid registration attempt");
                    return View(model);
                }

                var user = new UserEntity()
                {
                    UserName = model.Username,
                    Email = model.Email
                };
                var result = UserManager.Create(user, model.Password);

                if (!result.Succeeded)
                {
                    model.ExistingRoles = GetRolesForUser();
                    ModelState.AddModelError("", "Invalid registration attempt");
                    return View(model);
                }

                var userId = UserManager.FindByName(user.UserName).Id;

                var roleResult = UserManager.AddToRole(userId, model.UserRole);

                if (!roleResult.Succeeded)
                {
                    model.ExistingRoles = GetRolesForUser();
                    ModelState.AddModelError("", "Invalid registration attempt");
                    return View(model);
                }

                return RedirectToAction("Users", "Admin");
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        public ActionResult BlockUser(string userId)
        {
            _logger.Information("Action: BlockUser; Controller: Admin; Call method: POST;");
            try
            {
                if (!HasRights(userId))
                {
                    return View("Forbidden");
                }
                _adminService.BlockUser(userId);

                return RedirectToAction("Users", "Admin");
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        public ActionResult UnlockUser(string userId)
        {
            _logger.Information("Action: UnlockUser; Controller: Admin; Call method: POST;");
            try
            {
                if (!HasRights(userId))
                {
                    return View("Forbidden");
                }
                _adminService.UnblockUser(userId);

                return RedirectToAction("Users", "Admin");
            }
            catch (Exception e)
            {
                LogException(e);

                return RedirectToAction("Index", "Error", new { e.GetType().Name });
            }
        }
        private List<SelectListItem> GetRolesForUser()
        {
            var roles = RoleManager.Roles.ToList();
            List<string> rolesList = new List<string>
            {
                roles.Where(r => r.Name == "CommonUser").First().Name
            };

            if (User.IsInRole("Admin"))
            {
                rolesList.Add(roles.Where(r => r.Name == "Moderator").First().Name);
            }
            if (User.IsInRole("SuperAdmin"))
            {
                rolesList.Add(roles.Where(r => r.Name == "Moderator").First().Name);
                rolesList.Add(roles.Where(r => r.Name == "Admin").First().Name);
            }

            List<SelectListItem> selectListRoles = new List<SelectListItem>();

            foreach (var role in rolesList)
            {
                var selectRole = new SelectListItem()
                {
                    Value = role,
                    Text = role
                };

                selectListRoles.Add(selectRole);
            }

            return selectListRoles;
        }
        private bool HasRights(string userId)
        {
            var userRoleName = UserManager.GetRoles(userId).First();
            var userRoleId = Convert.ToInt32(RoleManager.Roles.Where(r => r.Name == userRoleName).First().Id);

            var currentUserRoleName = UserManager.GetRoles(User.Identity.GetUserId()).First();
            var currentUserRoleId = Convert.ToInt32(RoleManager.Roles.Where(r => r.Name == currentUserRoleName).First().Id);

            if (userRoleId > currentUserRoleId)
            {
                return true;
            }
            else
            {
                return false;
            }
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