namespace Security.Core.Models.UserManagement;

public class ListUsersResponse
{
    public List<UserSummaryDto> RegisteredUsers { get; set; }

    public bool Success { get; set; }

    public ListUsersResponse()
    {

    }
}
