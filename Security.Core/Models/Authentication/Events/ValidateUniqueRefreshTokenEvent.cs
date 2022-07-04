using MediatR;

namespace Security.Core.Models.UserManagement.Events;

public class ValidateUniqueRefreshTokenEvent : INotification
{
    public ValidateUniqueRefreshTokenEvent(Guid userId, Guid deviceId, string refreshToken)
    {
        UserId = userId;
        DeviceId = deviceId;
        RefreshToken = refreshToken;
    }

    public Guid UserId { get; }
    public Guid DeviceId { get; }
    public string RefreshToken { get; }
}