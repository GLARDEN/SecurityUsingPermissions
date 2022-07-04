using Ardalis.Specification;

using Security.Core.Models.UserManagement;

namespace Security.Core.Models.Administration.RoleManagement.Specifications;

public class GetRoleByIdSpec : Specification<Role>, ISingleResultSpecification
{
    public GetRoleByIdSpec(Guid roleId)
    {
        Query
            .Where(role => role.Id.Equals(roleId));
    }
}
