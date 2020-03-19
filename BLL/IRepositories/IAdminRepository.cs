using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface IAdminRepository
    {
        List<string> GetIds();
        void BlockUser(string userId);
        void UnblockUser(string userId);
    }
}
