using AutoMapper;

namespace ProjectTemplate.Extension
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                //cfg.AddProfile(new CustomProfile());
            });
        }
    }
}
