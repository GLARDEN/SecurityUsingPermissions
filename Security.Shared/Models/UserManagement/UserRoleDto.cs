﻿using Security.Shared.Models.Administration.RoleManagement;

namespace Security.Shared.Models.UserManagement;
public class UserRoleDto
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; }
    public string AssignedPermissions { get;  set; }

    public UserRoleDto() { }//Required by EF Core
    
}