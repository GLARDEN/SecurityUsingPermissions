namespace Security.Shared.Models.UserManagement;
public class AuthenticatedUser
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public byte[] PasswordHash { get;private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public IEnumerable<AssignedUserRole>? Roles { get; private set; }

    public AuthenticatedUser(string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public AuthenticatedUser(Guid Id, string email, byte[] passwordHash, byte[] passwordSalt, IEnumerable<AssignedUserRole>? assignedUserRoles )
    {
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        Roles = assignedUserRoles;
    }

    public void SetPasswordHash(byte[] passwordHash, byte[] passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }


}



