using Security.Shared.Permissions.Enums;

namespace Security.Shared.Models.Administration.Role;

public class CreateRoleRequest
{
    public const string Route = "api/administration/role/create";

    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}
