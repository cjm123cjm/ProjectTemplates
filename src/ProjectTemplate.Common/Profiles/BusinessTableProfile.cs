using AutoMapper;
using ProjectTemplate.Model.Tenants;
using ProjectTemplate.Model.Tenants.Dtos;

namespace ProjectTemplate.Common.Profiles
{
    public class BusinessTableProfile : Profile
    {
        public BusinessTableProfile()
        {
            CreateMap<BusinessTable, BusinessTableDto>().ReverseMap();
        }
    }
}
