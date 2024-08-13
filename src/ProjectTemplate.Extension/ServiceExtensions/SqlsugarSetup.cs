using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTemplate.Common.DB;
using ProjectTemplate.Common.HttpContextUser;
using ProjectTemplate.Model.Tenants;
using SqlSugar;

namespace ProjectTemplate.Extension.ServiceExtensions
{
    public static class SqlsugarSetup
    {
        public static IServiceCollection AddSqlsugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            var DbsOption = configuration.GetSection("DBS");
            var listdatabase = DbsOption.Get<List<MutiDBOperate>>()!.Where(t => t.Enabled).ToList();

            var mainDbId = configuration.GetValue<string>("MainDB");

            var mainDbModel = listdatabase.Single(d => d.ConnId == mainDbId);

            listdatabase.Remove(mainDbModel);
            listdatabase.Insert(0, mainDbModel);

            BaseDBConfig.MutiConnectionString = (listdatabase, mainDbModel.Slaves);

            // 默认添加主数据库连接
            if (!string.IsNullOrEmpty(mainDbId))
            {
                MainDb.CurrentDbConnId = mainDbId!;
            }

            foreach (var database in listdatabase)
            {
                var config = new ConnectionConfig
                {
                    ConfigId = database.ConnId.ToLower(),
                    ConnectionString = database.Connection,
                    DbType = (DbType)database.DbType,
                    IsAutoCloseConnection = true,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true,
                        SqlServerCodeFirstNvarchar = true,
                    },
                    InitKeyType = InitKeyType.Attribute
                };

                if (SqlSugarConst.LogConfigId.ToLower().Equals(database.ConnId.ToLower()))
                {
                    BaseDBConfig.LogConfig = config;
                }
                else
                {
                    BaseDBConfig.ValidConfig.Add(config);
                }

                BaseDBConfig.AllConfig.Add(config);
            }
            //if (BaseDBConfig.LogConfig is null)
            //{
            //    throw new ApplicationException("未配置Log库连接");
            //}

            // SqlSugarScope是线程安全，可使用单例注入
            // 参考：https://www.donet5.com/Home/Doc?typeId=1181
            services.AddSingleton<ISqlSugarClient>(o =>
            {
                //return new SqlSugarScope(BaseDBConfig.AllConfig);

                //多租户
                return new SqlSugarScope(BaseDBConfig.AllConfig, db =>
                {
                    BaseDBConfig.ValidConfig.ForEach(t =>
                    {
                        var dbProvider = db.GetConnectionScope((string)t.ConfigId);

                        var user = o.GetService<IUser>();
                        if (user != null && user.ID > 0 && user.TenantId > 0)
                        {
                            dbProvider.QueryFilter.AddTableFilter<ITenantEntity>(t => t.TenantId == user.TenantId || t.TenantId == 0);
                        }
                    });
                });
            });

            return services;
        }
    }
}
