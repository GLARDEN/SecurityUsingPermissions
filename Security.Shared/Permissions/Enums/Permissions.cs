using System.ComponentModel.DataAnnotations;

namespace Security.Shared.Permissions.Enums;
public enum Permission : short
{
    NotSet = 0,

    [Display(GroupName = "Forecast", Name = "View", Description = "Can view a list of all forecasts", Order = 10)]
    ForecastView = 10,
    [Display(GroupName = "Forecast", Name = "Create", Description = "Can create a new forecast", Order = 20)]    
    ForecastCreate = 11,    
    [Display(GroupName = "Forecast", Name = "Update", Description = "Can update a forecast", Order = 30)]
    ForecastUpdate = 12,
    [Display(GroupName = "Forecast", Name = "Delete", Description = "Can delete a forecast", Order = 40)]
    ForecastDelete = 13,


    [Display(GroupName = "UserManagement", Name = "Admin", Description = "Can do anything to the User",Order = 10)]
    UserAdmin = 20,
    [Display(GroupName = "UserManagement", Name = "Create", Description = "Can create a new user", Order = 20)]
    UserManagementAdd = 22,
    [Display(GroupName = "UserManagement", Name = "Update", Description = "Can update a users information", Order = 30)]
    UserManagementUpdate = 23,
    [Display(GroupName = "UserManagement", Name = "Delete", Description = "Can delete a user", Order = 40)]
    UserManagementDelete = 24,

    [Display(GroupName = "Admin", Name = "AccessAll", Description = "This allows the user to access every feature")]
    AccessAll = Int16.MaxValue
}
