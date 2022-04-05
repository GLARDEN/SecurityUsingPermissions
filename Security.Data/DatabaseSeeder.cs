using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using SecuredAPI.Services;

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
public class DatabaseCreator : IDatabaseCreator
{
    private readonly AppDbContext _appDbContext;

    public object PermissionsDisplay { get; private set; }

    public DatabaseCreator(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Initialize()
    {
        CreateRoles();
        CreateForecasts();
        CreateAdminUser();
        CreateTestUsers();
    }

    private void CreateRoles()
    {

        Task.Run(async () =>
        {
            List<Role> testRoleList = new();
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(typeof(Permission), true);

            List<string> forecastPermissions = permissionsToDisplay.Where(p => p.GroupName == "Forecast").Select(p => p.PermissionName).ToList();
            Role forecastUserRole = new("Forecast User", "Role allows users to work with forecast data", true, forecastPermissions);
            testRoleList.Add(forecastUserRole);


            List<string> userAdministrationPermissions = permissionsToDisplay.Where(p => p.GroupName == "User Management").Select(p => p.PermissionName).ToList();
            Role userAdministrationRole = new("User Administrator", "Role allows users to work with forecast data", true, userAdministrationPermissions);
            testRoleList.Add(userAdministrationRole);

            List<string> roleAdministrationPermissions = permissionsToDisplay.Where(p => p.GroupName == "Role Management").Select(p => p.PermissionName).ToList();
            Role roleAdministrationRole = new("Role Administrator", "Role allows users to work with forecast data", true, roleAdministrationPermissions);
            testRoleList.Add(roleAdministrationRole);


            await _appDbContext.Roles.AddRangeAsync(testRoleList);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateForecasts()
    {
        Task.Run(async () =>
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
            await _appDbContext.WeatherForecasts.AddRangeAsync(weatherForecasts);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateTestUsers()
    {
        Task.Run(async () =>
        {
            List<User> testUsers = new();
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(typeof(Permission), true);

            List<string> forecastPermissions = permissionsToDisplay.Where(p => p.GroupName == "Forecast").Select(p => p.PermissionName).ToList();

            //Test Forecast User Account
            CreatePasswordHash("forecast", out byte[] passwordHash, out byte[] passwordSalt);
            User forecastUser = new("forecastView@test.com", passwordHash, passwordSalt);

            forecastUser.AssignRole("Forecast User", forecastPermissions);

            //Test User Account Administrator Account
            List<string> userManagementPermissions = permissionsToDisplay.Where(p => p.GroupName == "User Management").Select(p => p.PermissionName).ToList();

            CreatePasswordHash("useradmin", out byte[] passwordHash1, out byte[] passwordSalt1);

            User userAdmin = new("um@test.com", passwordHash1, passwordSalt1);

            userAdmin.AssignRole("User Management", userManagementPermissions);


            //Test Role Administrator Account
            List<string> roleManagementPermissions = permissionsToDisplay.Where(p => p.GroupName == "Role Management").Select(p => p.PermissionName).ToList();

            CreatePasswordHash("roleadmin", out byte[] ph, out byte[] ps);

            User roleAdmin = new("roleAdmin@test.com", ph, ps);

            roleAdmin.AssignRole("Role Management", roleManagementPermissions);

            testUsers.Add(forecastUser);
            testUsers.Add(userAdmin);
            testUsers.Add(roleAdmin);
            await _appDbContext.Users.AddRangeAsync(testUsers);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateAdminUser()
    {
        Task.Run(async () =>
        {
            List<string> adminPermission = new List<string>() { Permission.AccessAll.ToString() };


            CreatePasswordHash("admin", out byte[] passwordHash, out byte[] passwordSalt);
            User admin = new("admin@test.com", passwordHash, passwordSalt);

            admin.AssignRole("Admin", adminPermission);

            await _appDbContext.Users.AddAsync(admin);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }
}

