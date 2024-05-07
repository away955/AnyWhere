namespace Xray.Models;

public sealed class XrayNodeSubModel : ViewModelBase
{
    [Reactive]
    public int Id { get; set; }
    [Reactive]
    public string Url { get; set; } = string.Empty;
    [Reactive]
    public bool IsDisable { get; set; }
    [Reactive]
    public string Remark { get; set; } = string.Empty;
}
