namespace Security.Shared.Models;
public class User
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;

    public DateTime CreatedWhen { get; set; } = DateTime.Now;

    public User(string email, byte[] passwordHash, byte[] passwordSalt)
    {
        Email = email;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }
}

 

