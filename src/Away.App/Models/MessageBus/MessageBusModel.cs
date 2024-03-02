namespace Away.App.Models;

public enum MessageBusType
{
    Shutdown,
    NavMainBox,
    WindowState,
    Notification,
    Event
}

public class MessageBusModel(MessageBusType messageType, object args)
{
    public MessageBusType MessageType { get; set; } = messageType;
    public object Args { get; set; } = args;
}
