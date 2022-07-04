
using System.Data;
using System.Security.Claims;

namespace Security.Core.Models.Authentication;

public class UserTokenDetails
{
    public Guid Id { get; set; }
    public Guid DeviceId { get; set; }  
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Permissions { get; set; } = null!;

}
