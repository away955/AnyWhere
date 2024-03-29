using Avalonia.Threading;

namespace Away.App.Core.Messages;

internal static class MessageBusExtensions
{
    public static void Subscribe(this IMessageBus bus, MessageBusType messageType, Action<object> action, string? contract = null)
    {
        bus.Listen<MessageBusModel>(contract)
            .Where(o => o.MessageType == messageType)
            .Subscribe(o => Dispatcher.UIThread.Post(() => action(o.Args)));
    }

    public static void Publish(this IMessageBus bus, MessageBusType messageType, object args, string? contract = null)
    {
        Dispatcher.UIThread.Post(() => bus.SendMessage(new MessageBusModel(messageType, args), contract));
    }
}
