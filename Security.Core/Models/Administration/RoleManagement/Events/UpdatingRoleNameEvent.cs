using MediatR;

namespace Security.Core.Models.Administration.RoleManagement.Events;

public class UpdatingRoleNameEvent : INotification
{
    public UpdatingRoleNameEvent(Guid id, string newName)
    {
        Id = id;
        NewName = newName;
    }
    public Guid Id { get; }
    public string NewName { get; }
}


