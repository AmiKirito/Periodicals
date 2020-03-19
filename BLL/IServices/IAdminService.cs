using System.Collections.Generic;

namespace BLL.IServices
{
    public interface IAdminService
    {
        List<string> GetUserIdList();
        void BlockUser(string userId);
        void UnblockUser(string userId);
    }
}
