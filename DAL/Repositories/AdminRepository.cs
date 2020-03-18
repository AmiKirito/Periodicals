using BLL.IRepositories;
using DAL.ModelsEntities;
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
    }
}
