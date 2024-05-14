namespace Xray.Models;

public sealed class XrayLogModel : ReactiveObject
{
    [Reactive]
    public string access { get; set; } = string.Empty;
    [Reactive]
    public string error { get; set; } = string.Empty;
    [Reactive]
    public string loglevel { get; set; } = string.Empty;
    [Reactive]
    public bool dnsLog { get; set; }
}
