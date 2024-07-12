namespace RouterScanner.Models;

public sealed class RouterModel : ReactiveObject
{
    /// <summary>
    /// 地址
    /// </summary>
    [Reactive]
    public string Url { get; set; } = string.Empty;
    /// <summary>
    /// 厂商|路由名称
    /// </summary>
    [Reactive]
    public string Production { get; set; } = string.Empty;
    /// <summary>
    /// 版本
    /// </summary>
    [Reactive]
    public string Version { get; set; } = string.Empty;
    /// <summary>
    /// 固件版本
    /// </summary>
    [Reactive]
    public string Firmware { get; set; } = string.Empty;
    /// <summary>
    /// 漏洞列表
    /// </summary>
    [Reactive]
    public List<IRouterVulHub> Vuls { get; set; } = [];
}
