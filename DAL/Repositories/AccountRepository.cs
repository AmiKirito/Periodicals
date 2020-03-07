using BLL.IRepositories;
using DAL.ModelsEntities;
using System.Linq;

namespace DAL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext _context;
        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }
        public void AddBalance(int amount, string username)
        {
            var user = _context.Users.OfType<UserEntity>().Where(u => u.UserName == username).First();

            user.AccountSum += amount;

            _context.SaveChanges();
        }
    }
}
