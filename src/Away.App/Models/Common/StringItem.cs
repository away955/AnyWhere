namespace Away.App.Models;

public sealed class StringItem : ViewModelBase
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
