using Ardalis.Specification;

namespace Security.Core.Models.Administration.RoleManagement.Specifications;
public class GetEnabledRolesSpec : Specification<Role>
{
    public GetEnabledRolesSpec()
    {
        Query.Where(role => role.Enabled);
    }
}
