using AutoMapper;
using ProjectTemplate.Model.Entity;

namespace ProjectTemplate.Common.Profiles
{
    public class SysUserProfile : Profile
    {
        public SysUserProfile()
        {
            CreateMap<SysUserInfo, SysUserInfo>();
        }
    }
}
