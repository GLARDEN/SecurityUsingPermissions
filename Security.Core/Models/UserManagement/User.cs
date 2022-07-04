using Ardalis.GuardClauses;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement.Events;
using Security.SharedKernel;
using Security.SharedKernel.Interfaces;

namespace Security.Core.Models.UserManagement;

public class User: IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public bool Enabled { get; private set; }
    public DateTime CreatedWhen { get; private set; } = DateTime.Now;

    private readonly List<UserRole> _userRoles = new List<UserRole>();
    public IEnumerable<UserRole> UserRoles => _userRoles;

    private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
    public List<RefreshToken> RefreshTokens => _refreshTokens;

    public User() { }

    public User(Guid id, string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Guard.Against.Default(id, nameof(id));
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public static User Create(string emailAddress, byte[] passwordHash, byte[] passwordSalt, bool enabled)
    {
        Guard.Against.NullOrEmpty(emailAddress, nameof(emailAddress), "Role must have a name.");

        User newUser = new();
        newUser.UpdateEmail(emailAddress);
        newUser.SetPasswordHash(passwordHash,passwordSalt);
        return newUser;
    }

    public void SetPasswordHash(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void UpdateEmail(string email)
    {
        DomainEvents.Raise(new ValidateUniqueUserEmailAddressEvent(Id,email)).Wait();
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
               
        if (UserRoles.Any(r => r.RoleName == roleName))
        {
            UserRole roleToUpdate = UserRoles.First(r => r.RoleName == roleName);
            if(roleToUpdate != null)
            {
                roleToUpdate.UpdatePermissions(permissions);
            }
        }
    }

    public void RevokeRole(UserRole userRole)
    {
        if (UserRoles.Any(ur => ur.RoleName.ToLower() == userRole.RoleName.ToLower()))
        {            
            _userRoles.Remove(userRole);
        }
    }

    public void SetRefreshToken(RefreshToken refreshToken)
    {
        DomainEvents.Raise(new ValidateUniqueRefreshTokenEvent(refreshToken.UserId, refreshToken.DeviceId,refreshToken.Token)).Wait();

        _refreshTokens.Add(refreshToken);
        
    }

    public void RevokeRefreshToken(Guid deviceId)
    {
        RefreshToken refreshToken = _refreshTokens.FirstOrDefault(t => t.DeviceId.Equals(deviceId));
        if (refreshToken != null)
        {
            refreshToken.MarkAsInvalid();
        }
    }

    public void RevokeAllRefreshTokens()
    {
        _refreshTokens.ForEach(t => t.MarkAsInvalid());
    }
}