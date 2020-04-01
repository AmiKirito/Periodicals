using System.Collections.Generic;

namespace BLL.IRepositories
{
    public interface IAdminRepository
    {
        /// <summary>
        /// Method that gets all user IDs
        /// </summary>
        List<string> GetIds();
        void BlockUser(string userId);
        void UnblockUser(string userId);
    }
}
