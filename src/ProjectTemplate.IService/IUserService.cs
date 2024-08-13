using ProjectTemplate.IService.Base;
using ProjectTemplate.Model.Entity;

namespace ProjectTemplate.IService
{
    public interface IUserService : IBaseService<SysUserInfo, SysUserInfo>
    {
        Task<List<RoleModulePermission>> RoleModuleMaps();
    }
}
