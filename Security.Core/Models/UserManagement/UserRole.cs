namespace Security.Core.Models.UserManagement;

public class UserRole
{
    public Guid UserId { get; private set; }
    public string RoleName { get; private set; }
    public string AssignedPermissions { get; private set; }


    public UserRole(Guid userId, string roleName, string assignedPermissions)
    {
        UserId = userId;
        RoleName = roleName;
        AssignedPermissions = assignedPermissions;
    }



    public void UpdatePermissions(string permissions)
    {
        AssignedPermissions = permissions;
    }

}
