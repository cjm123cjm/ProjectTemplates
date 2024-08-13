using AutoMapper;
using ProjectTemplate.IService;
using ProjectTemplate.Model.Entity;
using ProjectTemplate.Repository.Base;
using ProjectTemplate.Service.Base;

namespace ProjectTemplate.Service
{
    public class UserService : BaseService<SysUserInfo, SysUserInfo>, IUserService
    {
        public UserService(IBaseRepository<SysUserInfo> baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
        }

        public Task<List<RoleModulePermission>> RoleModuleMaps()
        {
            throw new NotImplementedException();
        }
    }
}
