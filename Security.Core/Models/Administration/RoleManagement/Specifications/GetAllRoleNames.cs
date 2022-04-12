using Ardalis.Specification;

namespace Security.Core.Models.Administration.RoleManagement.Specifications;
public class GetAllRoleNamesSpec : Specification<Role>
{
    public GetAllRoleNamesSpec(Guid roleId, string roleName)
    {
        Query
            .Where(role => !role.Id.Equals(roleId) &&
              role.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
    }
}
