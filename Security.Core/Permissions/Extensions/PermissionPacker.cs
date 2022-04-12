﻿// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Security.Core.Permissions.Enums;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Security.Core.Permissions.Extensions;

public static class PermissionPacker
{
    private static Type enumPermissionsType = typeof(Permission);
    public static string PackCommaDelimitedPermissionsNames(this Type enumPermissionsType, string permissionNames)
    {
        return enumPermissionsType.PackPermissionsNames(permissionNames.Split(',').Select(x => x.Trim()));
    }

    public static string PackPermissionsNames(this IEnumerable<string> permissionNames)
    {
        var packedPermissions = permissionNames.Aggregate("", (s, permissionName) =>
            s + (char)Convert.ChangeType(Enum.Parse(enumPermissionsType, permissionName), typeof(char)));
        CheckPackedPermissionsDoesNotContainZeroChar(packedPermissions);
        return packedPermissions;
    }

    public static string PackPermissionsNames(this Type enumPermissionsType, IEnumerable<string> permissionNames)
    {
        var packedPermissions = permissionNames.Aggregate("", (s, permissionName) =>
            s + (char)Convert.ChangeType(Enum.Parse(enumPermissionsType, permissionName), typeof(char)));
        CheckPackedPermissionsDoesNotContainZeroChar(packedPermissions);
        return packedPermissions;
    }

    /// <summary>
    /// This converts a list of enum permission names into a packed string. If any permission names are bad it calls the reportError action
    /// </summary>
    /// <param name="enumPermissionsType"></param>
    /// <param name="permissionNames"></param>
    /// <param name="reportError">Report a permission name that isn't in the list of enum members</param>
    /// <param name="foundAdvancedPermission">Only called if an advanced permission is found</param>
    /// <returns>the packed permission string</returns>
    public static string PackPermissionsNamesWithValidation(this Type enumPermissionsType,
        IEnumerable<string> permissionNames, Action<string> reportError, Action foundAdvancedPermission)
    {
        var packedPermissions = "";
        foreach (var permissionName in permissionNames)
        {
            try
            {
                Enum.Parse(enumPermissionsType, permissionName);
                var displayAttribute = enumPermissionsType.GetMember(permissionName)[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute?.GetAutoGenerateFilter() == true)
                    foundAdvancedPermission();
            }
            catch (ArgumentException)
            {
                reportError(permissionName);
                continue;
            }

            packedPermissions += (char)Convert.ChangeType(Enum.Parse(enumPermissionsType, permissionName), typeof(char));
        }
        CheckPackedPermissionsDoesNotContainZeroChar(packedPermissions);
        return packedPermissions;
    }

    //----------------------------------------------------------------------
    // private methods

    private static void CheckPackedPermissionsDoesNotContainZeroChar(string packedPermissions)
    {
        if (packedPermissions.Contains((char)0))
            throw new Exception(
                "A packed permissions string must not contain a char of zero value");
    }
}
