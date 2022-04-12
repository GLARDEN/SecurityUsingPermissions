using Microsoft.AspNetCore.Authorization;

namespace Security.Core.Authorization.Requirements;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permissionName)
    {
        PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
    }

    public string PermissionName { get; }
}
