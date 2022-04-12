using MediatR;

namespace Security.Core.Events;

public static class DomainEvents
{
    [ThreadStatic] // ensure separate func per thread to support parallel invocation
    public static Func<IMediator> Mediator;

    public static async Task Raise<T>(T args) where T : INotification
    {
        var mediator = Mediator.Invoke();
        await mediator.Publish(args);
    }
}
