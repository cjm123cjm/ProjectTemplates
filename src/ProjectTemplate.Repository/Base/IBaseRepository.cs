using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.Repository.Base
{
    public interface IBaseRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> QueryAsync();
    }
}
