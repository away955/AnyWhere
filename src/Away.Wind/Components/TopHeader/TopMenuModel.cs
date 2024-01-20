using System.Windows.Input;

namespace Away.Wind.Components;

public class TopMenuModel
{
    public required string ToolTip { get; set; }
    public required string Icon { get; set; }
    public required ICommand Command { get; set; }
}
