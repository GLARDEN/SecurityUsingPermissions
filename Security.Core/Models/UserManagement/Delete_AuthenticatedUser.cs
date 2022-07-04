namespace Security.Core.Models.UserManagement;
public class Delete_AuthenticatedUser
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public byte[] PasswordHash { get; private set; } = null!;
    public byte[] PasswordSalt { get; private set; } = null!;
    public IEnumerable<Delete_AssignedUserRole>? Roles { get; private set; }

    public Delete_AuthenticatedUser(string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public Delete_AuthenticatedUser(Guid Id, string email, byte[] passwordHash, byte[] passwordSalt, IEnumerable<Delete_AssignedUserRole>? assignedUserRoles)
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



