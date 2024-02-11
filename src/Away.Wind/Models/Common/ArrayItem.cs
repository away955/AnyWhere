namespace Away.Wind.Models;

public class StringItem : BindableBase
{
    private string _item = string.Empty;
    public string Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }
    public StringItem()
    {

    }
    public StringItem(string item)
    {
        Item = item;
    }
}
