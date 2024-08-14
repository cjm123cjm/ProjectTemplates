using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using ProjectTemplate.Common.Caches;
using ProjectTemplate.Common.DB;
using ProjectTemplate.Common.HttpContextUser;
using ProjectTemplate.Extension.ServiceExtensions;
using System.Text;

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

//注册AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
AutoMapperConfig.RegisterMappings();

//redis
var redis = builder.Configuration.GetSection("Redis");
builder.Services.Configure<RedisOption>(redis);
RedisOption redisOption = redis.Get<RedisOption>()!;
builder.Services.AddCacheSetup(redisOption);

//sqlsugar
var dbs = builder.Configuration.GetSection("DBS");
builder.Services.Configure<List<MutiDBOperate>>(dbs);
builder.Services.AddSqlsugarSetup(builder.Configuration);

//jwt
var secret = builder.Configuration.GetValue<string>("JwtOptions:Secret");
var issuer = builder.Configuration.GetValue<string>("JwtOptions:Issuer");
var audience = builder.Configuration.GetValue<string>("JwtOptions:Audience");
var key = Encoding.ASCII.GetBytes(secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        ValidateAudience = true
    };
});

//自定义授权策略
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Permission", policy => policy.Requirements.Add(new PermissionRequirement()));
});
builder.Services.AddScoped<IAuthorizationRequirement, PermissionRequirementHandler>();
builder.Services.AddSingleton(new PermissionRequirement());
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

//用户信息
builder.Services.AddScoped<IUser, AspNetUser>();

//日志


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
