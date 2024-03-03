namespace Away.App.Models;

public sealed class ComboBoxModel : ViewModelBase
{
    [Reactive]
    public string Text { get; set; } = string.Empty;
    [Reactive]
    public string ToolTip { get; set; } = string.Empty;
}
