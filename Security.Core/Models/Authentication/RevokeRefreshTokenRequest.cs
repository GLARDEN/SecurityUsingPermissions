namespace Security.Core.Models.Authentication;

public class RevokeRefreshTokenRequest
{
    public const string Route = "api/authentication/RevokeRefreshToken";
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; } 
    public bool RevokeAll { get; set; }
    public string? ErrorMessage { get; set; }

}
