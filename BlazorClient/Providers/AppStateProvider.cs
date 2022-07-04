namespace BlazorClient.Providers;

public class AppStateProvider<t> : IAppStateProvider<t>
{
    private t? _state;
    public t? State
    {
        get => _state;
        set
        {
            _state = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnStateChange;

    private void NotifyStateChanged() => OnStateChange?.Invoke();
}

