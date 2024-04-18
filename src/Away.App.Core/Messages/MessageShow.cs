using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using ReactiveUI;

namespace Away.App.Core.Messages;

/// <summary>
/// 系统消息
/// </summary>
public static class MessageShow
{
    public static void Success(string title, string? message = "")
    {
        Show(title, message, NotificationType.Success);
    }
    public static void Error(string title, string? message = "")
    {
        Show(title, message, NotificationType.Error);
    }
    public static void Info(string title, string? message = "")
    {
        Show(title, message, NotificationType.Information);
    }
    public static void Warning(string title, string? message = "")
    {
        Show(title, message, NotificationType.Warning);
    }
    public static void Show(string? title, string? message = "", NotificationType type = NotificationType.Information, TimeSpan? expiration = null, Action? onClick = null, Action? onClose = null)
    {
        Log.Information($"{title}\r\n{message}");
        Dispatcher.UIThread.Post(() =>
        {
            MessageBus.Current.Nofity(title, message, type, expiration, onClick, onClose);
        });
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
        bus.Publish(MessageBusType.MessageShow, new Notification(title, message, type, expiration, onClick, onClose));
    }
    public static void Listen(Action<object> action, string? contract = null)
    {
        MessageBus.Current.Subscribe(MessageBusType.MessageShow, action, contract);
    }
}
