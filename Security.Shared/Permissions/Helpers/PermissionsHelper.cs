// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.
using Security.Shared.Permissions.Enums;

namespace Security.Shared.Permissions.Helpers;

public static class PermissionsHelper
{
    public static string ConvertPermissionsToDelimitedString(this IEnumerable<Permission> permissions)
    {
        var result = permissions.Aggregate("", (s, permission) => s + (char)permission);
        return result;
    }

    public static IEnumerable<Permission> UnpackPermissionsFromString(this string permissionsString)
    {
        if (permissionsString == null)
            throw new ArgumentNullException(nameof(permissionsString));
        foreach (var character in permissionsString)
        {
            yield return ((Permission)character);
        }
    }

    public static Permission? FindPermissionViaName(this string permissionName)
    {
        return Enum.TryParse(permissionName, out Permission permission)
            ? (Permission?)permission
            : null;
    }
}
