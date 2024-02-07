namespace Away.Wind.Services;

public interface IMessageService
{
    void Show(string text);
    void Subscribe(Action<string> action);
}
