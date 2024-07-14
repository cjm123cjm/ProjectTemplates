using SqlSugar;
using System.Linq.Expressions;

namespace ProjectTemplate.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        private readonly ISqlSugarClient _dbBase;

        public BaseRepository(ISqlSugarClient dbBase)
        {
            _dbBase = dbBase;
        }

        public ISqlSugarClient Db => _dbBase;

        public async Task<long> AddAsync(TEntity entity)
        {
            var insert = _dbBase.Insertable(entity);
            return await insert.ExecuteReturnSnowflakeIdAsync();
        }

        public async Task<List<long>> AddSplitAsync(TEntity entity)
        {
            var insert = _dbBase.Insertable(entity).SplitTable();
            //插入并返回雪花ID并且自动赋值ID　
            return await insert.ExecuteReturnSnowflakeIdListAsync();
        }

        public async Task<List<TEntity>> QuerySplitAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {
            return await _dbBase.Queryable<TEntity>()
                 .SplitTable()
                 .OrderByIF(!string.IsNullOrWhiteSpace(orderByFields), orderByFields)
                 .WhereIF(whereExpression != null, whereExpression)
                 .ToListAsync();
        }

        public async Task<List<TEntity>> QueryAsync()
        {
            return await _dbBase.Queryable<TEntity>().ToListAsync();
        }
    }
}
