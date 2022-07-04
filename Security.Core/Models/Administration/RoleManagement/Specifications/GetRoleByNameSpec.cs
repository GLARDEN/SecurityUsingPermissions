using Ardalis.Specification;

namespace Security.Core.Models.Administration.RoleManagement.Specifications;

public class GetRoleByNameSpec : Specification<Role>, ISingleResultSpecification
{
    public GetRoleByNameSpec(string roleName)
    {
        Query
            .Where(role => role.Name.Equals(roleName,StringComparison.OrdinalIgnoreCase));
    }
}