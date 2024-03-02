namespace Away.App.Models;

public class CheckBoxGroupModel : ViewModelBase
{
    [Reactive]
    public bool IsChecked { get; set; }
    [Reactive]
    public string Content { get; set; } = string.Empty;
    [Reactive]
    public string ToolTip { get; set; } = string.Empty;
}

