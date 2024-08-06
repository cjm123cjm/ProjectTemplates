using AutoMapper;
using ProjectTemplate.IService;
using ProjectTemplate.Model.LogEntity;
using ProjectTemplate.Model.LogEntity.Dtos;
using ProjectTemplate.Repository.Base;
using ProjectTemplate.Service.Base;

namespace ProjectTemplate.Service
{
    public class AuditSqlLogService : BaseService<AuditSqlLog, AuditSqlLogDto>, IAuditSqlLogService
    {
        private readonly IBaseRepository<AuditSqlLog> _baseRepository;
        public AuditSqlLogService(IBaseRepository<AuditSqlLog> baseRepository, IMapper mapper) : base(baseRepository, mapper)
        {
            _baseRepository = baseRepository;
        }
    }
}
