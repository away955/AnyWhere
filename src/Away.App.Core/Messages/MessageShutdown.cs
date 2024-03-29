namespace Away.App.Core.Messages;

public static class MessageShutdown
{
    public static void Shutdown(string? contract = null)
    {
        MessageBus.Current.Publish(MessageBusType.Shutdown, new object(), contract);
    }

    public static void Listen(Action<object> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.Shutdown, action, contract);
    }
}
