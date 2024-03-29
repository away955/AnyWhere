namespace Away.App.Core.Messages;

/// <summary>
/// 菜单路由
/// </summary>
public static class MessageRouter
{
    public static void Go(string url, string? contract = null)
    {
        MessageBus.Current.Publish(MessageBusType.Routes, url, contract);
    }

    public static void Listen(Action<object> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.Routes, action, contract);
    }
}
