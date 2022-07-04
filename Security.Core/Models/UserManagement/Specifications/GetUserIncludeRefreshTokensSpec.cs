using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUserIncludeRefreshTokensSpec : Specification<User>, ISingleResultSpecification
{
    public GetUserIncludeRefreshTokensSpec(Guid userId)
    {
        Query
            .Where(user => user.Id.Equals(userId))          

            .Include(u =>u.RefreshTokens)
            .IgnoreQueryFilters();
    }
}
