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

namespace Periodicals.Controllers
{
    [Authorize(Roles = "Moderator,Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
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
            int pageSize = 2;
            var userIds = _adminService.GetUserIdList();
            int totalItems = userIds.Count();

            if (page > Math.Ceiling(((double)totalItems / (double)pageSize)) && totalItems != 0)
            {
                return View("NotFound");
            }

            var currentUserRoleName = UserManager.GetRoles(User.Identity.GetUserId()).First();
            var currentUserRoleId = Convert.ToInt32(RoleManager.Roles.Where(r => r.Name == currentUserRoleName).First().Id);
            List<Tuple<string, string, string, bool, bool>> preModel = new List<Tuple<string, string, string, bool, bool>>();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

            UsersViewModel model = new UsersViewModel();

            foreach (var userId in userIds)
            {
                var user = UserManager.FindById(userId);
                var userRoleName = UserManager.GetRoles(userId).First();
                var userRoleId = Convert.ToInt32(RoleManager.Roles.Where(r => r.Name == userRoleName).First().Id);
                bool isLocked = false;
                bool canLock = false;
                string username = UserManager.FindById(userId).UserName;
                string role = UserManager.GetRoles(userId).FirstOrDefault();

                if (user.LockoutEndDateUtc > DateTime.Now && user.LockoutEndDateUtc != null)
                {
                    isLocked = true;
                }
                if(userRoleId > currentUserRoleId)
                {
                    canLock = true;
                }

                preModel.Add(new Tuple<string, string, string, bool, bool>(userId, username, role, isLocked, canLock));
            }

            model.Items = preModel;
            model.Items = model.Items.Skip((page - 1) * pageSize).Take(pageSize);
            model.PageInfo = pageInfo;

            return View(model);
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            var roles = RoleManager.Roles.ToList();
            List<string> rolesList = new List<string>();
            CreateUserViewModel model = new CreateUserViewModel();

            rolesList.Add(roles.Where(r => r.Name == "CommonUser").First().Name);

            if (User.IsInRole("Admin"))
            {
                rolesList.Add(roles.Where(r => r.Name == "Moderator").First().Name);
            }                
            if(User.IsInRole("SuperAdmin"))
            {
                rolesList.Add(roles.Where(r => r.Name == "Moderator").First().Name);
                rolesList.Add(roles.Where(r => r.Name == "Admin").First().Name);
            }

            model.ExistingRoles = ConvertToSelectListRoles(rolesList);

            return View(model);
        }
        public ActionResult CreateUser(CreateUserViewModel model)
        {
            if(!ModelState.IsValid)
            {
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
                ModelState.AddModelError("errorRegisterAttempt", "Invalid registration attempt");
                return View(model);
            }

            var userId = UserManager.FindByName(user.UserName).Id;

            var roleResult = UserManager.AddToRole(userId, model.UserRole);

            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("errorRegisterAttempt", "Invalid registration attempt");
                return View(model);
            }

            return RedirectToAction("Users", "Admin");
        }
        public ActionResult BlockUser(string userId, bool canLock)
        {
            if(!canLock)
            {
                ViewBag.NoRight = "error";
                return View("Users");
            }
            _adminService.BlockUser(userId);

            return RedirectToAction("Users", "Admin");
        }
        public ActionResult UnblockUser(string userId, bool canUnlock)
        {
            if(!canUnlock)
            {
                ViewBag.NoRight = "error";
                return View("Users");
            }
            _adminService.UnblockUser(userId);

            return RedirectToAction("Users", "Admin");
        }
        public List<SelectListItem> ConvertToSelectListRoles(List<string> roles)
        {
            List<SelectListItem> selectListRoles = new List<SelectListItem>();

            foreach (var role in roles)
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
    }
}