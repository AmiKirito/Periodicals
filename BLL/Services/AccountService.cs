using BLL.IRepositories;
using BLL.IServices;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public void AddSumToBalance(int sum, string username)
        {
            _accountRepository.AddBalance(sum, username);
        }
    }
}
