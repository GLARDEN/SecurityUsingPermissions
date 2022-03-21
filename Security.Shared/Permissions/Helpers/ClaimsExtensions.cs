using Security.Shared.Permissions.Constants;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Security.Shared.Permissions.Helpers;
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
        return claims?.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
    }


    /// <summary>
    /// This returns the UserId from the current user's claims
    /// </summary>
    /// <param name="claims"></param>
    /// <returns>The UserId, or null if not logged in</returns>
    public static string GetUserIdFromClaims(this IEnumerable<Claim> claims)
    {
        return claims?.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// This returns the UserId from the current user 
    /// </summary>
    /// <param name="user">The current ClaimsPrincipal user</param>
    /// <returns>The UserId, or null if not logged in</returns>
    public static string GetUserIdFromUser(this ClaimsPrincipal user)
    {
        return user?.Claims.GetUserIdFromClaims();
    }


    /// <summary>
    /// This returns the AuthP packed permissions. Can be null if no user, or not packed permissions claims
    /// </summary>
    /// <param name="user">The current ClaimsPrincipal user</param>
    /// <returns>The packed permissions, or null if not logged in</returns>
    public static string GetPackedPermissionsFromUser(this ClaimsPrincipal user)
    {
        return user?.Claims.SingleOrDefault(x => x.Type == PermissionConstants.PackedPermissionClaimType)?.Value;
    }
}
