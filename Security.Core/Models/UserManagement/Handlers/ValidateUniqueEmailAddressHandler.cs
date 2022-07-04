using MediatR;

using Security.SharedKernel.Interfaces;
using Security.Core.Models.Administration.RoleManagement.Specifications;
using Security.Core.Models.Administration.RoleManagement.Events;
using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Events;
using Security.Core.Models.UserManagement.Specifications;

namespace Security.Core.Models.UserManagement.Handlers;
public class ValidateUniqueEmailAddressHandler : INotificationHandler<ValidateUniqueUserEmailAddressEvent>
{
    private readonly IRepository<User> _repository;

    public ValidateUniqueEmailAddressHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task Handle(ValidateUniqueUserEmailAddressEvent notification, CancellationToken cancellationToken)
    {
        var checkForUsersWithSameEmailSpec = new CheckForUsersWithSameEmailSpec(notification.Id, notification.NewEmailAddress);

        var foundDuplicateEmailAddress = await _repository.AnyAsync(checkForUsersWithSameEmailSpec);

        if (foundDuplicateEmailAddress)
        {
            throw new Exception($"{notification.NewEmailAddress} already exists. Duplicate email addresses not allowed.");
        }
    }
}
