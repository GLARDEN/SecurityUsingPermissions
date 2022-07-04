using MediatR;

namespace Security.Core.Models.UserManagement.Events;

public class CheckForInvalidPermissionEvent : INotification
{
    public CheckForInvalidPermissionEvent(string permissions)
    {
        Permissions = permissions;
    }
    public string Permissions { get; }
}


