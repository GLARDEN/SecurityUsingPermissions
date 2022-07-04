namespace Security.Core.Models.Authentication;

public class LogOutRequest
{
    public const string Route = "api/authentication/logout";
    public Guid UserId { get; set; } 
    public Guid DeviceId { get; set; }
    
}
