﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProjectTemplate.Common.HttpContextUser
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<AspNetUser> _logger;
        public AspNetUser(IHttpContextAccessor contextAccessor, ILogger<AspNetUser> logger)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public string Name => GetName();
        private string GetName()
        {
            if (IsAuthenticated() && _contextAccessor.HttpContext.User.Identity.Name.IsNotEmptyOrNull())
            {
                return _contextAccessor.HttpContext.User.Identity.Name;
            }
            return "";
        }
        public bool IsAuthenticated()
        {
            return _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
        public long ID => GetClaimValueByType("jti").FirstOrDefault().ObjToLong();

        public long TenantId => GetClaimValueByType("TenantId").FirstOrDefault().ObjToLong();

        public string GetToken()
        {
            var token = _contextAccessor.HttpContext?.Request?.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
            if (!token.IsNullOrEmpty())
            {
                return token;
            }

            return token;
        }

        public List<string> GetUserInfoFromToken(string ClaimType)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = "";

            token = GetToken();
            // token校验
            if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(token);

                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }

            return new List<string>() { };
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            if (_contextAccessor.HttpContext == null) return ArraySegment<Claim>.Empty;

            if (!IsAuthenticated()) return GetClaimsIdentity(GetToken());

            var claims = _contextAccessor.HttpContext.User.Claims.ToList();
            var headers = _contextAccessor.HttpContext.Request.Headers;
            foreach (var header in headers)
            {
                claims.Add(new Claim(header.Key, header.Value));
            }

            return claims;
        }

        public IEnumerable<Claim> GetClaimsIdentity(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            // token校验
            if (token.IsNotEmptyOrNull() && jwtHandler.CanReadToken(token))
            {
                var jwtToken = jwtHandler.ReadJwtToken(token);

                return jwtToken.Claims;
            }

            return new List<Claim>();
        }

        public List<string> GetClaimValueByType(string ClaimType)
        {
            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();
        }
    }
}
