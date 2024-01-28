namespace Away.Service.Xray.Model;

/// <summary>
/// 内置的 DNS 服务器. 如果没有配置此项，则使用系统的 DNS 设置。
/// </summary>
public class XrayDns
{
    public Dictionary<string, string>? hosts { get; set; }
    public List<string>? servers { get; set; }
    public string? clientIp { get; set; }
    public string? queryStrategy { get; set; }
    public string? disableCache { get; set; }
    public string? disableFallback { get; set; }
    public string? disableFallbackIfMatch { get; set; }
    public string? tag { get; set; }
}

public class XrayDnsServer
{
    public string? address { get; set; }
    public int port { get; set; }
    public List<string>? domains { get; set; }
    public List<string>? expectIPs { get; set; }
    public bool skipFallback { get; set; }
    public string? clientIP { get; set; }
}