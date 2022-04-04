using Security.Shared.Permissions.Enums;

namespace Security.Shared.Models.Administration.Role;

public class CreateRoleRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}
