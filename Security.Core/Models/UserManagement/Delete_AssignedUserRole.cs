using Ardalis.GuardClauses;

namespace Security.Core.Models.UserManagement;
public class Delete_AssignedUserRole
{
    public Guid UserId { get; private set; }
    public string RoleName { get; private set; }
    public string PermissionsInRole { get; private set; }

    private Delete_AssignedUserRole() { }//Required by EF Core


    public Delete_AssignedUserRole(Guid userId, string roleName, string permissions)
    {
        Guard.Against.NullOrEmpty(userId, nameof(userId), "User role requires a user id.");
        Guard.Against.NullOrEmpty(roleName, nameof(roleName), "User role must have a role name.");
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "Assigned user role must have atleast 1 permission assigned.");

        UserId = userId;
        RoleName = roleName;
        PermissionsInRole = permissions;
    }

    public void UpdatePermissions(string permissions)
    {
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "Assigned user role must have atleast 1 permission assigned.");
        PermissionsInRole = permissions;
    }
}
