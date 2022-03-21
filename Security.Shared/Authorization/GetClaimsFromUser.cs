using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using Security.Shared.Authorization;
using Security.Shared.Permissions.Constants;
using Security.Shared.Permissions.Helpers;

namespace SecuredAPI.Authorization;

public class GetClaimsFromUser : IGetClaimsProvider
{
    public string UserId { get; private set; }
    public GetClaimsFromUser(IHttpContextAccessor accessor)
    {
        UserId = accessor.HttpContext?.User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        var permissionsClaim = accessor.HttpContext?.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);

        var test = permissionsClaim?.Value.UnpackPermissionsFromString();
    }
}
