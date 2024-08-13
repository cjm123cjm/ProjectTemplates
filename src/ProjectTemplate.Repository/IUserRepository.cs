using ProjectTemplate.Model.Entity;

namespace ProjectTemplate.Repository
{
    public interface IUserRepository
    {
        Task<List<RoleModulePermission>> RoleModuleMaps();
    }
}
