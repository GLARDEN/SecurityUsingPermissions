using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models;

public class EditUserRolesRequest
{
    public Guid UserId { get; set; }

    public Dictionary<string,IEnumerable<string>> Roles { get; set; } = null!;
  
}