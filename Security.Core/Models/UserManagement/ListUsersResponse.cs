namespace Security.Core.Models.UserManagement;

public class ListUsersResponse
{
    public List<UserDto> RegisteredUsers { get; set; }

    public bool Success { get; set; }

    public ListUsersResponse()
    {

    }
}
