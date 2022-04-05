using System.ComponentModel.DataAnnotations;

namespace Security.Shared.Permissions.Enums;
public enum Permission : ushort
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


    [Display(GroupName = "User Management", Name = "View", Description = "Has full control of user information",Order = 10)]
    UserView = 20,
    [Display(GroupName = "User Management", Name = "Create", Description = "Can create a new user", Order = 20)]
    UserAdd = 22,
    [Display(GroupName = "User Management", Name = "Edit", Description = "Can edit a users information", Order = 30)]
    UserEdit = 23,
    [Display(GroupName = "User Management", Name = "Delete", Description = "Can delete a user", Order = 40)]
    UserDelete = 24,

    [Display(GroupName = "User Role Management", Name = "View", Description = "Can view a users roles", Order = 50)]
    UserRoleView = 30,
    [Display(GroupName = "User Role Management", Name = "Create", Description = "Can view a users roles", Order = 60)]
    UserRoleCreate = 31,
    [Display(GroupName = "User Role Management", Name = "Edit", Description = "Can edit a users role", Order = 70)]
    UserRoleEdit = 32,
    [Display(GroupName = "User Role Management", Name = "Delete", Description = "Can delete a users role", Order = 80)]
    UserRoleDelete = 33,

    [Display(GroupName = "Role Management", Name = "View", Description = "Can view a list of all roles", Order = 90)]
    RoleView = 40,
    [Display(GroupName = "Role Management", Name = "Create", Description = "Can create a new role", Order = 100)]
    RoleCreate = 41,
    [Display(GroupName = "Role Management", Name = "Edit", Description = "Can update a role", Order = 110)]
    RoleEdit = 42,
    [Display(GroupName = "Role Management", Name = "Delete", Description = "Can delete a role", Order = 120)]
    RoleDelete = 43,

    [Display(GroupName = "Admin", Name = "AccessAll", Description = "This allows the user to access every feature")]
    AccessAll = 4567
}
