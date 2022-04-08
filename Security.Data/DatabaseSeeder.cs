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
        CreateDefaultRoles();
        CreateForecasts();
        CreateDefaultUsers();
    }

    private void CreateDefaultRoles()
    {
        Task.Run(async () =>
        { 
            await _appDbContext.Roles.AddRangeAsync(SecuritySetUpData.DefaultRoleDefinitions);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateForecasts()
    {
        Task.Run(async () =>
        {
            await _appDbContext.WeatherForecasts.AddRangeAsync(SecuritySetUpData.GetSampleForecastData());
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

    private void CreateDefaultUsers()
    {
        Task.Run(async () =>
        {
            await _appDbContext.Users.AddRangeAsync(SecuritySetUpData.GetDefaultUsers());
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }

}

