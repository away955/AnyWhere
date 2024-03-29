namespace Away.App.Core.Messages;

public static class MessageEvent
{
    public static void Run(object e, string? contract = null)
    {
        MessageBus.Current.Publish(MessageBusType.Event, e, contract);
    }
    public static void Listen(Action<object> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.Event, action, contract);
    }
}
