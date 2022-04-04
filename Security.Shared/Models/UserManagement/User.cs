using Ardalis.GuardClauses;

using Security.Shared.Permissions.Extensions;

namespace Security.Shared.Models.UserManagement;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public DateTime CreatedWhen { get; private set; } = DateTime.Now;
    public IEnumerable<UserRole> UserRoles { get; private set; } 

    public User() { }

    public User(string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public User(string email, byte[] passwordHash, byte[] passwordSalt, IEnumerable<UserRole> userRoles)
    {
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        UserRoles = userRoles;
    }

    public void SetPasswordHash(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void UpdateEmail(string email)
    {
        Email = email;
    }

    public void AssignRole(string roleName,List<string> permissionNames) 
    {
        Guard.Against.NullOrEmpty(roleName,nameof(roleName), "Role Name can not be empty.");
        Guard.Against.NullOrEmpty(permissionNames, nameof(permissionNames), "User Role must have atleast one permission assigned");

        if(!UserRoles.Any(ur => ur.RoleName.ToLower() == roleName.ToLower()))
        {
            UserRole newRole = new(roleName,permissionNames.PackPermissionsNames());
        }
    }

    public void UpdateRole(string roleName, string permissions)
    {
        Guard.Against.NullOrEmpty(roleName, nameof(roleName), "Role Name can not be empty.");
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "User Role must have atleast one permission assigned");


        UserRole roleToUpdate = UserRoles.FirstOrDefault(r => r.RoleName == roleName);
        if (roleToUpdate != null)
        {
            roleToUpdate.UpdatePermissions(permissions);
        }

    }
}