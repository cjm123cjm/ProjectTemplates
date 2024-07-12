using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTemplate.IService.Base
{
    public interface IBaseService<TEntity, TDto> where TEntity : class, new() where TDto : class
    {
        Task<TDto> Query();
    }
}
