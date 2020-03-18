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
        public ActionResult Users()
        {
            var userIds = _adminService.GetUserIdList();

            List<Tuple<string, string, string, bool>> model = new List<Tuple<string, string, string, bool>>();

            foreach (var userId in userIds)
            {
                bool isLocked = false;
                string username = UserManager.FindById(userId).UserName;
                string role = UserManager.GetRoles(userId).FirstOrDefault();
                if (UserManager.FindById(userId).LockoutEndDateUtc > DateTime.Now)
                {
                    isLocked = true;
                }
                model.Add(new Tuple<string, string, string, bool>(userId, username, role, isLocked));
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult CreateUser()
        {
            return View();
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
        public ActionResult BlockUser(string userId)
        {
            var user = UserManager.FindById(userId);

            user.LockoutEndDateUtc = DateTime.MaxValue;
            
            return RedirectToAction("Users", "Admin");
        }
        public ActionResult UnblockUser(string userId)
        {
            var user = UserManager.FindById(userId);

            user.LockoutEndDateUtc = null;

            return RedirectToAction("Users", "Admin");
        }
    }
}