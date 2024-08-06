using ProjectTemplate.Model.LogEntity.Dtos;

namespace ProjectTemplate.IService
{
    public interface IAuditSqlLogService
    {
        Task<List<AuditSqlLogDto>> Query();
    }
}
