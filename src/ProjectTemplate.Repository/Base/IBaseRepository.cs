using SqlSugar;
using System.Linq.Expressions;

namespace ProjectTemplate.Repository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        ISqlSugarClient Db { get; }
        Task<long> AddAsync(TEntity entity);
        Task<List<long>> AddSplitAsync(TEntity entity);
        Task<List<TEntity>> QueryAsync();
        Task<List<TEntity>> QuerySplitAsync(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null);
    }
}
