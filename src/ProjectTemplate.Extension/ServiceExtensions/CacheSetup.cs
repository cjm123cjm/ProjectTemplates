using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ProjectTemplate.Common.Config;
using ProjectTemplate.Extension.Redis;
using ProjectTemplate.Common.Caches;

namespace ProjectTemplate.Extension.ServiceExtensions
{
    public static class CacheSetup
    {
        public static IServiceCollection AddCacheSetup(this IServiceCollection services, RedisOption redisOption)
        {
            if (redisOption.Enable)
            {
                // 配置启动Redis服务，虽然可能影响项目启动速度，但是不能在运行的时候报错，所以是合理的
                var configuration = ConfigurationOptions.Parse(redisOption.ConnectionString, true);
                configuration.ResolveDns = true;
                var connectionMultiplexer = ConnectionMultiplexer.Connect(configuration);

                services.AddSingleton<IConnectionMultiplexer>(sp =>
                {
                    return connectionMultiplexer;
                });

                services.AddSingleton<ConnectionMultiplexer>(p => p!.GetRequiredService<IConnectionMultiplexer>()! as ConnectionMultiplexer);

                services.AddStackExchangeRedisCache(setup =>
                {
                    setup.ConnectionMultiplexerFactory =
                        () => Task.FromResult(connectionMultiplexer as IConnectionMultiplexer);

                    if (!string.IsNullOrEmpty(redisOption.InstanceName))
                        setup.InstanceName = redisOption.InstanceName;
                });

                services.AddTransient<IRedisBasketRepository, RedisBasketRepository>();
            }
            else
            {
                //使用内存
                services.AddMemoryCache();
                services.AddDistributedMemoryCache();
            }

            services.AddTransient<ICaching, Caching>();

            return services;
        }
    }
}
