namespace ProjectTemplate.Repository.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        public Task<TEntity> QueryAsync()
        {
            throw new NotImplementedException();
        }
    }
}
