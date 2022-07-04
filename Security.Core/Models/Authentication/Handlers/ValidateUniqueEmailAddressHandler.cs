using MediatR;

using Security.SharedKernel.Interfaces;
using Security.Core.Models.Administration.RoleManagement.Specifications;
using Security.Core.Models.Administration.RoleManagement.Events;
using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Events;
using Security.Core.Models.UserManagement.Specifications;
using Security.Core.Models.Authentication;

namespace Security.Core.Models.UserManagement.Handlers;
public class ValidateUniqueRefreshTokenHandler : INotificationHandler<ValidateUniqueRefreshTokenEvent>
{
    private readonly IRepository<User> _repository;

    public ValidateUniqueRefreshTokenHandler(IRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task Handle(ValidateUniqueRefreshTokenEvent notification, CancellationToken cancellationToken)
    {
        var checkForUniqueRefreshTokenSpec = new CheckForUniqueRefreshTokenSpec(notification.UserId, notification.DeviceId, notification.RefreshToken);

        var foundDuplicateEmailAddress = await _repository.AnyAsync(checkForUniqueRefreshTokenSpec);

        if (foundDuplicateEmailAddress)
        {
            throw new Exception($"Device is already associated with valid refresh token");
        }
    }
}
