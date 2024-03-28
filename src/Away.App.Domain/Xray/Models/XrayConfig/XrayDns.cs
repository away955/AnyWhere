namespace Away.Domain.Xray.Models;

/// <summary>
/// 内置的 DNS 服务器. 如果没有配置此项，则使用系统的 DNS 设置。
/// </summary>
public sealed class XrayDns
{
    /// <summary>
    /// DNS 服务器列表
    /// </summary>
    public List<string> servers { get; set; } = [];
    /// <summary>
    /// 禁用 DNS 缓存
    /// </summary>
    public bool disableCache {  get; set; }
    /// <summary>
    /// 禁用 DNS 回退
    /// </summary>
    public bool disableFallback { get; set; }   

    public static XrayDns Default
    {
        get
        {
            return new XrayDns
            {
                servers = ["1.1.1.1", "8.8.8.8"]
            };
        }
    }
}
