using MediatR;

using Security.Core.Events;
using Security.Core.Models.Administration.RoleManagement.Specifications;
using Security.SharedKernel.Interfaces;

namespace Security.Core.Models.Administration.RoleManagement.Handlers;
public class ValidateUniqueRoleNameNameHandler : INotificationHandler<UpdatingNameEvent>
{
    private readonly IRepository<Role> _roleRepository;

    public ValidateUniqueRoleNameNameHandler(IRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task Handle(UpdatingNameEvent notification, CancellationToken cancellationToken) 
    {
        var spec = new GetAllRoleNamesSpec(notification.Id, notification.NewName);

        var foundDuplicateRoleName = await _roleRepository.AnyAsync(spec);

        if (foundDuplicateRoleName)
        {
            throw new Exception($"{notification.NewName} already exists. Role Duplicate role names not allowed.");
        }
    }
}
