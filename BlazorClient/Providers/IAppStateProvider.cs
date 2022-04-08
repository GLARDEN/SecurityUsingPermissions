
namespace BlazorClient.Providers;

public interface IAppStateProvider<t>
{
    t State { get; set; }

    event Action? OnStateChange;
}