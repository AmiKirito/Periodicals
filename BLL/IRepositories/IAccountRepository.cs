namespace BLL.IRepositories
{
    public interface IAccountRepository
    {
        void AddBalance(int amount, string username);
    }
}
