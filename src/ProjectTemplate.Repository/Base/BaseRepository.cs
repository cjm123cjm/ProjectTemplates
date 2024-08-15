using ProjectTemplate.Common.DB;
using ProjectTemplate.Model.Tenants;
using ProjectTemplate.Repository.UnitOfWorks;
using SqlSugar;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectTemplate.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        //private readonly ISqlSugarClient _dbBase;

        //public BaseRepository(ISqlSugarClient dbBase)
        //{
        //    _dbBase = dbBase;
        //}

        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private long _tenantId;
        private SqlSugarScope _dbBase;
        protected ISqlSugarClient _db
        {
            get
            {
                ISqlSugarClient db = _dbBase;

                //切库
                var tenantAttr = typeof(TEntity).GetCustomAttribute<TenantAttribute>();
                if (tenantAttr != null)
                {
                    db = _dbBase.GetConnectionScope(tenantAttr.configId.ToString().ToLower());
                    return db;
                }

                //多租户
                var mta = typeof(TEntity).GetCustomAttribute<MultiTenantAttribute>();
                if (mta != null && mta.TenantTypeEnum == TenantTypeEnum.Db)
                {
                    if (_tenantId > 0)
                    {
                        var tenant = db.Queryable<SysTenant>().WithCache().Where(t => t.Id == _tenantId).First();
                        if (tenant != null)
                        {
                            var itenant = db.AsTenant();
                            //切库
                            if (!itenant.IsAnyConnection(tenant.ConfigId))
                            {
                                itenant.AddConnection(tenant.GetConnectionConfig());
                            }
                            return itenant.GetConnectionScope(tenant.ConfigId);
                        }
                    }
                }

                return db;
            }
        }
        public ISqlSugarClient Db => _db;
        public BaseRepository(IUnitOfWorkManage unitOfWorkManage)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _dbBase = _unitOfWorkManage.GetDbClient();
            _tenantId = _unitOfWorkManage.TenantId;
        }
        public async Task<long> AddAsync(TEntity entity)
        {
            var insert = _db.Insertable(entity);
            return await insert.ExecuteReturnSnowflakeIdAsync();
        }

        public async Task<List<TEntity>> QueryAsync()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 查询缓存
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryWithCacheAsync()
        {
            return await _db.Queryable<TEntity>().WithCache().ToListAsync();
        }

        /// <summary>
        /// 分表添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<List<long>> AddSplitAsync(TEntity entity)
        {
            var insert = _db.Insertable(entity).SplitTable();
            //插入并返回雪花ID并且自动赋值ID　
            return await insert.ExecuteReturnSnowflakeIdListAsync();
        }

        /// <summary>
        /// 分表查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QuerySplitAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {
            return await _db.Queryable<TEntity>()
                 .SplitTable()
                 .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields)
                 .WhereIF(whereExpression != null, whereExpression)
                 .ToListAsync();
        }
        /// <summary>
        /// 分表查询缓存
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        public async Task<List<TEntity>> QuerySplitWithCacheAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {
            return await _db.Queryable<TEntity>()
                 .SplitTable()
                 .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields)
                 .WhereIF(whereExpression != null, whereExpression)
                 .WithCache()
                 .ToListAsync();
        }

    }
}
