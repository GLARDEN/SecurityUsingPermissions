using Ardalis.GuardClauses;

namespace Security.Core.Models.UserManagement;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public DateTime CreatedWhen { get; private set; } = DateTime.Now;
    private readonly List<UserRole> _userRoles = new List<UserRole>();
    public IEnumerable<UserRole> UserRoles => _userRoles;

    public User() { }

    public User(Guid id, string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Guard.Against.Default(id, nameof(id));
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
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

    public void AssignRole(string roleName, string permissionNames)
    {
        Guard.Against.NullOrEmpty(roleName, nameof(roleName), "Role Name can not be empty.");
        Guard.Against.NullOrEmpty(permissionNames, nameof(permissionNames), "User Role must have atleast one permission assigned");

        if (!UserRoles.Any(ur => ur.RoleName.ToLower() == roleName.ToLower()))
        {
            var permissions = permissionNames;
            UserRole newRole = new(Id, roleName, permissions);

            _userRoles.Add(newRole);
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