using AutoMapper;
using ProjectTemplate.Common.Profiles;

namespace ProjectTemplate.Extension.ServiceExtensions
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new SysUserProfile());
            });
        }
    }
}
