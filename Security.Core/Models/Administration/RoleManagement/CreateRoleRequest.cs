namespace Security.Core.Models.Administration.RoleManagement;

public class CreateRoleRequest
{
    public const string Route = "api/administration/roleManagement/create";

    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public IEnumerable<string> Permissions { get; set; } = new List<string>();

    public CreateRoleRequest()
    {

    }
}
