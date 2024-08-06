using AutoMapper;
using ProjectTemplate.IService.Base;
using ProjectTemplate.Repository.Base;
using SqlSugar;
using System.Linq.Expressions;

namespace ProjectTemplate.Service.Base
{
    public class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto> where TEntity : class, new() where TDto : class
    {
        private readonly IBaseRepository<TEntity> _baseRepository;
        private readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        public ISqlSugarClient Db => _baseRepository.Db;

        public Task<long> Add(TEntity entity)
        {
            return _baseRepository.AddAsync(entity);
        }

        /// <summary>
        /// 分表添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<List<long>> AddSplit(TEntity entity)
        {
            return _baseRepository.AddSplitAsync(entity);
        }

        /// <summary>
        /// 分表查询
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <param name="orderByFields"></param>
        /// <returns></returns>
        public async Task<List<TDto>> QuerySplit(Expression<Func<TEntity, bool>> whereExpression, string orderByFields = null)
        {
            var entities = await _baseRepository.QuerySplitAsync(whereExpression, orderByFields);
            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<List<TDto>> Query()
        {
            var entities = await _baseRepository.QueryAsync();
            return _mapper.Map<List<TDto>>(entities);
        }
    }
}
