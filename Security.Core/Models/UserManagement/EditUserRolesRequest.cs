
namespace Security.Core.Models.UserManagement;

public class EditUserRolesRequest
{
    public Guid UserId { get; set; }

    public Dictionary<string, IEnumerable<string>> Roles { get; set; } = null!;

}