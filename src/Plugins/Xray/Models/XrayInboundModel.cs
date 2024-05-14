namespace Xray.Models;

public sealed class XrayInboundModel : ReactiveObject
{
    [Reactive]
    public string? listen { get; set; }

    [Reactive]
    public int? port { get; set; }
    [Reactive]
    public string protocol { get; set; } = "http";
    /// <summary>
    /// 此入站连接的标识 唯一
    /// </summary>
    [Reactive]
    public string tag { get; set; } = string.Empty;
}
