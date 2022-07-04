using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUsersIncludeAssignedRolesAndTokensSpec : Specification<User>, ISingleResultSpecification
{
    public GetUsersIncludeAssignedRolesAndTokensSpec()
    {
        Query.Include(user => user.UserRoles)
            .Include(user => user.RefreshTokens)
            .IgnoreQueryFilters();
    }
}
