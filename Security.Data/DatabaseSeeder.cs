using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using SecuredAPI.Services;

using Security.Shared.Models;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Enums;
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

        CreateForecasts();
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
            var permissionsToDisplay = PermissionDisplay.GetPermissionsToDisplay(typeof(Permission),true);

            List<string> forecastPermissions = permissionsToDisplay.Where(p => p.GroupName == "Forecast").Select(p => p.PermissionName).ToList();

            
            CreatePasswordHash("forecast", out byte[] passwordHash, out byte[] passwordSalt);
            User forecastUser = new("forecastView@test.com", passwordHash, passwordSalt);

            forecastUser.AssignRole("Forecast User", forecastPermissions);

            testUsers.Add(forecastUser);
                     

            List<string> userManagementPermissions = permissionsToDisplay.Where(p => p.GroupName == "User Management").Select(p => p.PermissionName).ToList();


            CreatePasswordHash("useradmin", out byte[] passwordHash1, out byte[] passwordSalt1);

            User userAdmin = new("userAdmin@test.com", passwordHash1, passwordSalt1);

            forecastUser.AssignRole("User Management", userManagementPermissions);




            await _appDbContext.Users.AddRangeAsync(testUsers);




            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateAdminUser()
    {
        Task.Run(async () =>
        {
            List<string> adminPermission = new List<string>() { Permission.AccessAll.ToString()};

            
            CreatePasswordHash("admin", out byte[] passwordHash, out byte[] passwordSalt);
            User admin = new("admin@test.com",passwordHash,passwordSalt);

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

