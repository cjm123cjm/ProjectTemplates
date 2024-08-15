using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectTemplate.Common;
using ProjectTemplate.Common.Caches;
using ProjectTemplate.Common.DB;
using ProjectTemplate.Common.HttpContextUser;
using SqlSugar;
using System.Text.RegularExpressions;

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

            var sp = services.BuildServiceProvider();
            ICaching caching = sp.GetRequiredService<ICaching>();
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
                    InitKeyType = InitKeyType.Attribute,
                    // 自定义特性(缓存)
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        DataInfoCacheService = new SqlSugarCacheService(caching),
                    },
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
            _ = services.AddSingleton<ISqlSugarClient>(o =>
            {
                //return new SqlSugarScope(BaseDBConfig.AllConfig);

                //多租户
                return new SqlSugarScope(BaseDBConfig.AllConfig, db =>
                {
                    BaseDBConfig.ValidConfig.ForEach(t =>
                    {
                        var dbProvider = db.GetConnectionScope((string)t.ConfigId);

                        //多租户
                        var user = o.GetService<IUser>();
                        RepositorySetting.SetTenantEntityFilter(dbProvider, user);

                        // 打印SQL语句
                        dbProvider.Aop.OnLogExecuting = (s, parameters) =>
                        {
                            SqlSugarAop.OnLogExecuting(dbProvider, user?.Name.ObjToString(), ExtractTableName(s),
                                Enum.GetName(typeof(SugarActionType), dbProvider.SugarActionType), s, parameters,
                                t);
                        };
                    });
                });
            });

            return services;
        }

        private static string ExtractTableName(string sql)
        {
            // 匹配 SQL 语句中的表名的正则表达式
            //string regexPattern = @"\s*(?:UPDATE|DELETE\s+FROM|SELECT\s+\*\s+FROM)\s+(\w+)";
            string regexPattern = @"(?i)(?:FROM|UPDATE|DELETE\s+FROM)\s+`(.+?)`";
            Regex regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(sql);

            if (match.Success)
            {
                // 提取匹配到的表名
                return match.Groups[1].Value;
            }
            else
            {
                // 如果没有匹配到表名，则返回空字符串或者抛出异常等处理
                return string.Empty;
            }
        }
    }
}
