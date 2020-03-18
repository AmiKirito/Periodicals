using BLL.IRepositories;
using BLL.IServices;
using System.Collections.Generic;

namespace BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public List<string> GetUserIdList()
        {
            return _adminRepository.GetIds();
        }
    }
}
