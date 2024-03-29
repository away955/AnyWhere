namespace Away.App.Core.Messages;

public enum MessageBusType
{
    /// <summary>
    /// 关机
    /// </summary>
    Shutdown,
    /// <summary>
    /// 菜单路由
    /// </summary>
    Routes,
    /// <summary>
    /// 窗口状态
    /// </summary>
    WindowState,
    /// <summary>
    /// 系统消息
    /// </summary>
    MessageShow,
    /// <summary>
    /// 事件
    /// </summary>
    Event
}

public sealed class MessageBusModel(MessageBusType messageType, object args)
{
    public MessageBusType MessageType { get; set; } = messageType;
    public object Args { get; set; } = args;
}
