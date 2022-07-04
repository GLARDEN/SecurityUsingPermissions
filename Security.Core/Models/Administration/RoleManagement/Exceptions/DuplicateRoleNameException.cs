namespace Security.Core.Models.Administration.RoleManagement.Exceptions;

public class DuplicateRoleNameException : Exception
{
    public DuplicateRoleNameException(string message,string roleName) : base(message)
    {
        RoleName = roleName;
    }

    public string RoleName { get; }
}
