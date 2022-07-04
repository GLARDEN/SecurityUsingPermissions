using MediatR;

namespace Security.Core.Models.UserManagement.Events;

public class ValidateUniqueUserEmailAddressEvent : INotification
{
    public ValidateUniqueUserEmailAddressEvent(Guid id, string newEmailAddress)
    {
        Id = id;
        NewEmailAddress = newEmailAddress;
    }

    public Guid Id { get; }
    public string NewEmailAddress { get; }

}