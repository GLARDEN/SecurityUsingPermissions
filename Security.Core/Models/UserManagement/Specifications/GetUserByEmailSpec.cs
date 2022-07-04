using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUserByEmailSpec : Specification<User>, ISingleResultSpecification
{
    public GetUserByEmailSpec(string email)
    {
        Query.Include(u => u.UserRoles)             
            .Where(u => u.Email.ToLower() == email.ToLower());
    }
}
