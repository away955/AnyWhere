namespace Away.App.Core.Messages;

public enum MessageBusType
{
    Shutdown,
    NavMainBox,
    WindowState,
    Notification,
    Event
}

public sealed class MessageBusModel(MessageBusType messageType, object args)
{
    public MessageBusType MessageType { get; set; } = messageType;
    public object Args { get; set; } = args;
}
