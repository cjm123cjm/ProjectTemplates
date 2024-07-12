using ProjectTemplate.IService.Base;

namespace ProjectTemplate.Service
{
    public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto> where TEntity : class, new() where TDto : class
    {
        public Task<TDto> Query()
        {
            throw new NotImplementedException();
        }
    }
}
