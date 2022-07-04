using Ardalis.Specification;

namespace Security.Core.Models.Administration.RoleManagement.Specifications;
public class CheckForRolesWithSameNameSpec : Specification<Role>
{
    public CheckForRolesWithSameNameSpec(Guid roleId, string roleName)
    {
        Query
            .Where(role => !role.Id.Equals(roleId) && role.Name.ToLower() == roleName.ToLower());
    }
}
