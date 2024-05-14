namespace Xray.Models;

public sealed class ComboBoxModel: ReactiveObject
{
    [Reactive]
    public string Text { get; set; } = string.Empty;
    [Reactive]
    public string ToolTip { get; set; } = string.Empty;
}
