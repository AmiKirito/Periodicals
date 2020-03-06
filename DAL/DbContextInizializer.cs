using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using DAL.ModelsEntities;
using System.Linq;

namespace DAL
{
    public class DbContextInizializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            PublisherEntity pub1 = new PublisherEntity();
            pub1.Id = (context.Publishers.Count() + 1).ToString();
            pub1.Title = "Test";

            context.Publishers.Add(pub1);
            context.SaveChanges();


            CreateDefaultRoles(context);
        }

        private void CreateDefaultRoles(AppDbContext context)
        {
            var store = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(store);
            var userManager = new UserManager<UserEntity>(new UserStore<UserEntity>(context));

            var superAdmin = new UserEntity
            {
                UserName = "AmiKirito",
                Email = "romankrol2000@gmail.com"
            };
            string userPass = "qwerty123456";

            var superAdminRole = new IdentityRole()
            {
                Id = "1",
                Name = "SuperAdmin"
            };
            var adminRole = new IdentityRole()
            {
                Id = "2",
                Name = "Admin"
            };
            var moderatorRole = new IdentityRole()
            {
                Id = "3",
                Name = "Moderator"
            };
            var commonUserRole = new IdentityRole()
            {
                Id = "4",
                Name = "CommonUser"
            };

            List<IdentityRole> roles = new List<IdentityRole>();
            roles.Add(superAdminRole);
            roles.Add(adminRole);
            roles.Add(moderatorRole);
            roles.Add(commonUserRole);

            foreach (IdentityRole role in roles)
            {
                roleManager.Create(role);
            }

            context.SaveChanges();

            var result = userManager.Create(superAdmin, userPass);

            if (result.Succeeded)
            {
                userManager.AddToRole(superAdmin.Id, superAdminRole.Name);
            }

            context.SaveChanges();
        }
    }
} 

