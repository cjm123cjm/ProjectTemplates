using ProjectTemplate.Common.HttpContextUser;
using ProjectTemplate.Model;
using ProjectTemplate.Model.Tenants;
using SqlSugar;

namespace ProjectTemplate.Common.DB
{
    public static class RepositorySetting
    {
        public static void SetTenantEntityFilter(SqlSugarScopeProvider db, IUser user)
        {
            //多租户 单表字段
            if (user != null && user.ID > 0 && user.TenantId > 0)
            {
                db.QueryFilter.AddTableFilter<ITenantEntity>(t => t.TenantId == user.TenantId || t.TenantId == 0);
            }

            //多租户多表
            db.SetTenantTable(user.TenantId.ToString());
        }
        private static readonly Lazy<IEnumerable<Type>> AllEntities = new(() =>
        {
            return typeof(RootEntityTkey<>).Assembly
                    .GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract)
                    .Where(t => t.FullName != null && t.FullName.StartsWith("ProjectTemplate.Model"));
        });

        public static IEnumerable<Type> Entitys => AllEntities.Value;
    }
}
