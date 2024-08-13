using Microsoft.AspNetCore.Authorization;

namespace ProjectTemplate.Extension.ServiceExtensions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public List<PermissionItem> Permissions { get; set; } = new List<PermissionItem>();
    }
}
