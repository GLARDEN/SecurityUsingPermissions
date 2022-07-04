using MediatR;

using Security.SharedKernel.Interfaces;
using Security.Core.Models.Administration.RoleManagement.Specifications;
using Security.Core.Models.Administration.RoleManagement.Events;
using Security.Core.Models.Administration.RoleManagement.Exceptions;

namespace Security.Core.Models.Administration.RoleManagement.Handlers;
public class ValidateUniqueRoleNameHandler : INotificationHandler<UpdatingRoleNameEvent>
{
    private readonly IRepository<Role> _repository;

    public ValidateUniqueRoleNameHandler(IRepository<Role> repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdatingRoleNameEvent notification, CancellationToken cancellationToken)
    {
            var checkForRolesWithSameNameSpec = new CheckForRolesWithSameNameSpec(notification.Id, notification.NewName);

        var foundDuplicateRoleName = await _repository.AnyAsync(checkForRolesWithSameNameSpec);

        if (foundDuplicateRoleName)
        {
            throw new DuplicateRoleNameException($"{notification.NewName} already exists. Role Duplicate role names not allowed.", notification.NewName);
        }
    }
}
