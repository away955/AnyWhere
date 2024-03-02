using Avalonia.Controls.Notifications;

namespace Away.App.Extensions;

public static class MessageBusExtensions
{
    public static void Subscribe(this IMessageBus bus, MessageBusType messageType, Action<object> action, string? contract = null)
    {
        bus.Listen<MessageBusModel>(contract).Where(o => o.MessageType == messageType).Subscribe(o => action(o.Args));
    }

    public static void Publish(this IMessageBus bus, MessageBusType messageType, object args, string? contract = null)
    {
        bus.SendMessage(new MessageBusModel(messageType, args), contract);
    }

    /// <summary>
    /// 系统通知
    /// </summary>
    /// <param name="bus"></param>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="type"></param>
    /// <param name="expiration"></param>
    /// <param name="onClick"></param>
    /// <param name="onClose"></param>
    public static void Nofity(this IMessageBus bus, string? title, string? message, NotificationType type = NotificationType.Information, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        bus.Publish(MessageBusType.Notification, new Notification(title, message, type, expiration, onClick, onClose));
    }
}
