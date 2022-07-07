namespace Security.Core.Models.Authentication;

public class RefreshTokenDto
{
    public Guid UserId { get; set; }
    public Guid DeviceId { get;  set; }
    public string? DeviceName { get; set; }
    public string? Token { get;  set; } 
    public DateTime Expiry { get;  set; }
    public DateTime Created { get;  set; }
    public bool IsValid { get;  set; }
}