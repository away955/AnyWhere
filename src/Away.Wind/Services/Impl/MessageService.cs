using Prism.Events;

namespace Away.Wind.Services.Impl;

[ServiceInject]
public class MessageService : IMessageService
{
    private readonly MessageEvent _event;
    public MessageService(IEventAggregator eventAggregator)
    {
        _event = eventAggregator.GetEvent<MessageEvent>();
    }

    public void Show(string text)
    {
        _event.Publish(text);
    }

    public void Subscribe(Action<string> action)
    {
        _event.Subscribe(action);
    }
}

public class MessageEvent : PubSubEvent<string>
{
}
