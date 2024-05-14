namespace Xray.Models;

public sealed class StringItem : ReactiveObject
{
    [Reactive]
    public string Item { get; set; } = string.Empty;
    public StringItem()
    {

    }
    public StringItem(string item)
    {
        Item = item;
    }
}
