using Security.Core.Models.UserManagement;
using Security.Core.Permissions.Constants;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Permissions.Helpers;
/// <summary>
/// This contains extension method about ASP.NET Core <see cref="Claim"/>
/// </summary>
public static class ClaimsExtensions
{
    /// <summary>
    /// This returns the UserName from the current user's claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>The UserName, or null if not logged in</returns>
    public static string GetUserNameFromClaims(this IEnumerable<Claim> claims)
    {
        return claims?.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value ?? string.Empty;
    }
    /// <summary>
    /// This returns the Device Id from the current user's claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>The UserName, or null if not logged in</returns>
    public static Guid GetDeviceIdFromClaims(this IEnumerable<Claim> claims)
    {        
        Guid.TryParse(claims.SingleOrDefault(x => x.Type == ClaimsConstants.DeviceId)?.Value ?? string.Empty, out Guid deviceId);
        return deviceId;
    }

    /// <summary>
    /// This returns the UserId from the current user's claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>The UserId, or null if not logged in</returns>
    public static Guid GetUserIdFromClaims(this IEnumerable<Claim> claims)
    {
        Guid.TryParse(claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty,out Guid userId);
        return userId;
    }

    /// <summary>
    /// This returns the UserId from the current user 
    /// </summary>
    /// <param name="user">The current ClaimsPrincipal user</param>
    /// <returns>The UserId, or null if not logged in</returns>
    public static Guid GetUserIdFromUser(this ClaimsPrincipal user)
    {
        return user.Claims.GetUserIdFromClaims();
    }

    /// <summary>
    /// This returns the Device Id from the current user's claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>The UserName, or null if not logged in</returns>
    public static Guid GetDeviceIdFromUser(this ClaimsPrincipal user)
    {
        return user.Claims.GetDeviceIdFromClaims();
    }

    /// <summary>
    /// This returns the AuthP packed permissions. Can be null if no user, or not packed permissions claims
    /// </summary>
    /// <param name="user">The current ClaimsPrincipal user</param>
    /// <returns>The packed permissions, or null if not logged in</returns>
    public static string GetPackedPermissionsFromUser(this ClaimsPrincipal user)
    {
        return user?.Claims.SingleOrDefault(x => x.Type == PermissionConstants.PackedPermissionClaimType)?.Value ?? string.Empty;
    }
}
