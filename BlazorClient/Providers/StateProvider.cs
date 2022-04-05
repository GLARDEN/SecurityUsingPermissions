namespace BlazorClient.Providers;

public class StateProvider
{
    private object _state;

    public object State
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
