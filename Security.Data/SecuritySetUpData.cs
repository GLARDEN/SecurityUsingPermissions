using Security.Shared.Models;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Extensions;
using Security.Shared.Permissions.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Security.Data;
public class SecuritySetUpData
{
    public static readonly List<Role> DefaultRoleDefinitions = new List<Role>()
    {
        new("SuperAdmin", "Super admin - only use for setup",true, new List<string>(){Permission.AccessAll.ToString() }.PackPermissionsNames()),
        new("Forecast Manager", "Overall Forecast Permissions",true,
            new List<string>(){Permission.ForecastCreate.ToString(),Permission.ForecastUpdate.ToString(),Permission.ForecastDelete.ToString(),Permission.ForecastView.ToString() }.PackPermissionsNames()),
        new("User Manager", "User Manager can add , update or remove any user", true,
            new List<string>(){Permission.UserAdd.ToString(),Permission.UserEdit.ToString(),Permission.UserDelete.ToString(),Permission.UserView.ToString() }.PackPermissionsNames()),
        new("Role Management", "Role Manager can add , update or remove roles", true,
            new List<string>(){Permission.RoleCreate.ToString(),Permission.RoleEdit.ToString(),Permission.RoleDelete.ToString(),Permission.RoleView.ToString() }.PackPermissionsNames()),

    };

    public static List<WeatherForecast> GetSampleForecastData()
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        var weatherForecasts = new List<WeatherForecast>();

        for (int index = 0; index <= summaries.Length; index++)
        {
            var weatherForecast = new WeatherForecast(
               summaries[Random.Shared.Next(summaries.Length)],
               DateTime.Now.AddDays(index),
               Random.Shared.Next(-20, 55)
            );
            weatherForecasts.Add(weatherForecast);
        }

        return weatherForecasts;

    }

    public static List<User> GetDefaultUsers()
    {
        List<User> defaultUsers = new();
        var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(typeof(Permission), true);

        List<string> forecastPermissions = permissionsToDisplay.Where(p => p.GroupName == "Forecast").Select(p => p.PermissionName).ToList();

        //Test Forecast User Account
        CreatePasswordHash("forecast", out byte[] passwordHash, out byte[] passwordSalt);
        User forecastUser = new(Guid.NewGuid(), "forecastView@test.com", passwordHash, passwordSalt);

        var forcasetRoles = DefaultRoleDefinitions.FirstOrDefault(r => r.Name == "Forecast Manager")?.PermissionsInRole;
        forecastUser.AssignRole("Forecast Manager", forcasetRoles);

        //Test User Account Administrator Account
        CreatePasswordHash("useradmin", out byte[] passwordHash1, out byte[] passwordSalt1);
        User userAdmin = new(Guid.NewGuid(), "um@test.com", passwordHash1, passwordSalt1);
        var userManagementPermissions = DefaultRoleDefinitions.FirstOrDefault(r => r.Name == "User Manager")?.PermissionsInRole;
        userAdmin.AssignRole("User Management", userManagementPermissions);

        //Test Role Administrator Account        
        CreatePasswordHash("roleadmin", out byte[] ph, out byte[] ps);
        User roleAdmin = new(Guid.NewGuid(), "roleAdmin@test.com", ph, ps);
        var roleManagementPermissions = DefaultRoleDefinitions.FirstOrDefault(p => p.Name == "Role Management")?.PermissionsInRole;
        roleAdmin.AssignRole("Role Management", roleManagementPermissions);

        defaultUsers.Add(forecastUser);
        defaultUsers.Add(userAdmin);
        defaultUsers.Add(roleAdmin);

        return defaultUsers;
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

}
