namespace Away.App.Core.Messages;

public enum WindowStateCommandType : byte
{
    Normal = 0,
    Minimized = 1,
    Maximized = 2,
    FullScreen = 3,
    Hide = 4,
    Show = 5,
    Activate = 6,
    Close = 7,
    ShowActivate = 8,
}

/// <summary>
/// 窗口状态
/// </summary>
public static class MessageWindowState
{
    public static void State(WindowStateCommandType state, string? contract = null)
    {
        MessageBus.Current.Publish(MessageBusType.WindowState, state, contract);
    }

    public static void Listen(Action<object> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.WindowState, action, contract);
    }
}
