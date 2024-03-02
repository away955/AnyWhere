namespace Away.App.Models;

public class XrayDnsModel : ViewModelBase
{
    /// <summary>
    /// DNS 服务器列表
    /// </summary>
    [Reactive]
    public List<string> servers { get; set; } = [];
    /// <summary>
    /// 禁用 DNS 缓存
    /// </summary>
    [Reactive]
    public bool disableCache { get; set; }
    /// <summary>
    /// 禁用 DNS 回退
    /// </summary>
    [Reactive]
    public bool disableFallback { get; set; }
}
