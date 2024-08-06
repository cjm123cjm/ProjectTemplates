using AutoMapper;
using ProjectTemplate.Model.LogEntity;
using ProjectTemplate.Model.LogEntity.Dtos;

namespace ProjectTemplate.Common.Profiles
{
    public class AuditSqlLogProfile : Profile
    {
        public AuditSqlLogProfile()
        {
            CreateMap<AuditSqlLog, AuditSqlLogDto>().ReverseMap();
        }
    }
}
