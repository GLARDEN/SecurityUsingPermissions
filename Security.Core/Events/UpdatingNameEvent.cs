using MediatR;

namespace Security.Core.Events;

public class UpdatingNameEvent : INotification
{
    public UpdatingNameEvent(Guid id, string newName)
    {
        Id = id;
        NewName = newName;
    }

    public Guid Id { get; }
    public string NewName { get; }

}