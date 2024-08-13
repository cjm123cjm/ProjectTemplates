using ProjectTemplate.Model.Entity;
using ProjectTemplate.Repository.Base;
using ProjectTemplate.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Repository
{
    public class UserRepository : BaseRepository<SysUserInfo>, IUserRepository
    {
        public UserRepository(IUnitOfWorkManage unitOfWorkManage) : base(unitOfWorkManage)
        {
        }

        public async Task<List<RoleModulePermission>> RoleModuleMaps()
        {
            return await _db.Queryable<RoleModulePermission>()
                   .LeftJoin<Role>((o, cus) => o.RoleId == cus.Id)
                   .LeftJoin<Modules>((o, cus, oritem) => o.Id == oritem.Id)
                   .Where((o, cus, oritem) => o.IsDeleted == false && cus.IsDeleted == false && oritem.IsDeleted == false)
                   .Select((o, cus, oritem) => new RoleModulePermission
                   {
                       Role = cus,
                       Module = oritem,
                       IsDeleted = o.IsDeleted
                   }).ToListAsync();
        }
    }
}
