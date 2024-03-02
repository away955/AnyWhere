namespace Away.App.Models;

public class XrayInboundModel : ViewModelBase
{
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
