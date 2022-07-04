using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUserByIdSpec : Specification<User>, ISingleResultSpecification
{
    public GetUserByIdSpec(Guid userId)
    {
        Query
            .Where(user => user.Id.Equals(userId));
    }
}
