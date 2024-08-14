using ProjectTemplate.Model.Tenants;
using SqlSugar;
using System.Reflection;

namespace ProjectTemplate.Common.DB
{
    public static class TenantUtil
    {
        public static void SetTenantTable(this ISqlSugarClient db, string id)
        {
            var types = GetTenantEntityTypes(TenantTypeEnum.Table);

            foreach (var type in types)
            {
                db.MappingTables.Add(type.Name, type.GetTenantTableName(db, id));
            }
        }
        public static string GetTenantTableName(this Type type, ISqlSugarClient db, string id)
        {
            var entityInfo = db.EntityMaintenance.GetEntityInfo(type);
            return $@"{entityInfo.DbTableName}_{id}";
        }
        public static List<Type> GetTenantEntityTypes(TenantTypeEnum? tenantType = null)
        {
            return RepositorySetting.Entitys
                .Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass)
                .Where(s => IsTenantEntity(s, tenantType))
                .ToList();
        }

        private static bool IsTenantEntity(Type s, TenantTypeEnum? tenantType)
        {
            var mta = s.GetCustomAttribute<MultiTenantAttribute>();
            if (mta is null)
            {
                return false;
            }

            if (tenantType != null)
            {
                if (mta.TenantTypeEnum != tenantType)
                {
                    return false;
                }
            }

            return true;
        }
        public static ConnectionConfig GetConnectionConfig(this SysTenant tenant)
        {
            if (tenant.DbType is null)
            {
                throw new ArgumentException("Tenant DbType Must");
            }

            return new ConnectionConfig()
            {
                ConfigId = tenant.ConfigId,
                DbType = tenant.DbType.Value,
                ConnectionString = tenant.Connection,
                IsAutoCloseConnection = true,
                MoreSettings = new ConnMoreSettings()
                {
                    IsAutoRemoveDataCache = true,
                    SqlServerCodeFirstNvarchar = true,
                },
            };
        }

    }
}
