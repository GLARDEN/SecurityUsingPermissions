using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.UserManagement;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions.Enums;
using Security.Core.Permissions.Extensions;
using Security.Core.Permissions.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Security.Infrastructure;
public class SecuritySetUpData
{
    public static readonly List<Role> DefaultRoleDefinitions = new List<Role>()
    {
        Role.Create("SuperAdmin", "Super admin - only use for setup",true, new List<string>(){Permission.AccessAll.ToString() }.PackPermissionsNames()),
        Role.Create("Forecast Manager", "Overall Forecast Permissions",true,
            new List<string>(){Permission.ForecastCreate.ToString(),Permission.ForecastUpdate.ToString(),Permission.ForecastDelete.ToString(),Permission.ForecastView.ToString() }.PackPermissionsNames()),
        Role.Create("User Administrator", "User Administrator can add , update or remove users and roles", true,
            new List<string>(){Permission.RoleCreate.ToString(),Permission.RoleEdit.ToString(),Permission.RoleDelete.ToString(),Permission.RoleView.ToString(),
                               Permission.UserRoleCreate.ToString(),Permission.UserRoleEdit.ToString(),Permission.UserRoleDelete.ToString(),Permission.UserRoleView.ToString(),
                               Permission.UserView.ToString(),Permission.UserAdd.ToString(),Permission.UserEdit.ToString(),Permission.UserDelete.ToString() }.PackPermissionsNames())

    };

    public static List<Forecast> GetSampleForecastData()
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        var weatherForecasts = new List<Forecast>();

        for (int index = 0; index <= summaries.Length; index++)
        {
            var weatherForecast = new Forecast(
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
        var userAdministratorPermissions = DefaultRoleDefinitions.FirstOrDefault(r => r.Name == "User Administrator")?.PermissionsInRole;
        userAdmin.AssignRole("User Administrator", userAdministratorPermissions ?? String.Empty);


        defaultUsers.Add(forecastUser);
        defaultUsers.Add(userAdmin);        

        return defaultUsers;
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

}
