using MediatR;

using Security.Core.Models.UserManagement.Events;
using Security.Core.Models.UserManagement.Exceptions;
using Security.Core.Permissions.Extensions;
using Security.Core.Permissions.Services;

namespace Security.Core.Models.UserManagement.Handlers;
public class ValidateRequestedPemissionHandler : INotificationHandler<CheckForInvalidPermissionEvent>
{

    private readonly IPermissionService _permissionService;

    public ValidateRequestedPemissionHandler(IPermissionService permissionService)
    {

        _permissionService = permissionService;
    }

    public async Task Handle(CheckForInvalidPermissionEvent notification, CancellationToken cancellationToken)
    {
        var requestedPermissions = notification.Permissions.ConvertPackedPermissionToNames();

        List<string> invalidPermissionNames = _permissionService.ValidatePermissionName(requestedPermissions);

        if (invalidPermissionNames.Any())
        {
            throw new InvalidPermissionException($"The following requested permissions are invalid: {string.Join(",", invalidPermissionNames)}");
        }

        return;
    }
}
