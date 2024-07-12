using Autofac;
using Autofac.Extensions.DependencyInjection;
using ProjectTemplate.Common.Config;
using ProjectTemplate.Extension.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

//autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule<AutofacModuleRegister>();
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//×¢²áAutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
AutoMapperConfig.RegisterMappings();

//redis
var redis = builder.Configuration.GetSection("Redis");
builder.Services.Configure<RedisOption>(redis);
RedisOption redisOption = redis.Get<RedisOption>();
builder.Services.AddCacheSetup(redisOption);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
