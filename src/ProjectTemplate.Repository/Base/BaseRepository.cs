using SqlSugar;

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

        public Task<TEntity> QueryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
