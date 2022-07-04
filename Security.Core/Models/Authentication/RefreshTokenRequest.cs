namespace Security.Core.Models.Authentication;

public class RefreshTokenRequest
{
    public const string Route = "api/authentication/RefreshToken";
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
    public string? JwtToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? ErrorMessage { get; set; }
}
