namespace Xray.Services.Models;

/// <summary>
/// FakeDNS 配置。可配合透明代理使用，以获取实际域名。
/// </summary>
public sealed class XrayFakedns
{
    public string? ipPool { get; set; }
    public int poolSize { get; set; }
}
