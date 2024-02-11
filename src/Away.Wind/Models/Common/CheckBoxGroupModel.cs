namespace Away.Wind.Models;

public class CheckBoxGroupModel : BindableBase
{
    private bool _isChecked;
    private string _content = string.Empty;
    private string _toolTip = string.Empty;

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }

    public string Content
    {
        get => _content;
        set => SetProperty(ref _content, value);
    }

    public string ToolTip
    {
        get => _toolTip;
        set => SetProperty(ref _toolTip, value);
    }
}
