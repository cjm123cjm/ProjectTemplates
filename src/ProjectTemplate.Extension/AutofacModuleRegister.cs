using Autofac;
using ProjectTemplate.IService.Base;
using ProjectTemplate.Repository.Base;
using ProjectTemplate.Service;
using System.Reflection;

namespace ProjectTemplate.Extension
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            var serviceDllFile = Path.Combine(basePath, "ProjectTemplate.Service");
            var repositoryDllFile = Path.Combine(basePath, "ProjectTemplate.Repository");

            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>))
                .InstancePerDependency(); //注册仓储
            builder.RegisterGeneric(typeof(BaseService<,>)).As(typeof(IBaseService<,>))
                .InstancePerDependency(); //注册服务

            //获取 Service.dll 程序集服务,并注册
            var assemblyServices = Assembly.LoadFile(serviceDllFile);
            builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();

            //获取 Repository.dll 程序集服务,并注册
            var assemblyRepository = Assembly.LoadFile(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblyRepository)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .PropertiesAutowired();
        }
    }
}
