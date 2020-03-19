using BLL.IRepositories;
using DAL.ModelsEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;

        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public void BlockUser(string userId)
        {
            var user = _context.Users.OfType<UserEntity>().Where(u => u.Id == userId).First();

            user.LockoutEndDateUtc = DateTime.MaxValue;

            _context.SaveChanges();
        }

        public List<string> GetIds()
        {
            var users= _context.Users.OfType<UserEntity>().ToList();

            List<string> userIdList = new List<string>();

            foreach (var user in users)
            {
                userIdList.Add(user.Id);
            }

            return userIdList;
        }

        public void UnblockUser(string userId)
        {
            var user = _context.Users.OfType<UserEntity>().Where(u => u.Id == userId).First();

            user.LockoutEndDateUtc = null;

            _context.SaveChanges();
        }
    }
}
