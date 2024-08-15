using SqlSugar;
using System.Linq.Expressions;

namespace ProjectTemplate.IService.Base
{
    public interface IBaseService<TEntity, TDto> where TEntity : class, new() where TDto : class
    {
        ISqlSugarClient Db { get; }

        Task<long> Add(TEntity entity);
        Task<List<long>> AddSplit(TEntity entity);
        Task<List<TDto>> Query();
        Task<List<TDto>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null);
        Task<List<TDto>> QueryWithCache();
    }
}
