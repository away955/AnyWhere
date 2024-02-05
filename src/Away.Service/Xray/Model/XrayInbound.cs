namespace Away.Service.Xray.Model;

/// <summary>
/// 入站连接配置
/// </summary>
public class XrayInbound
{
    /// <summary>
    /// 监听地址，只允许 IP 地址，默认值为 "0.0.0.0"
    /// </summary>
    public string? listen { get; set; }
    public required int port { get; set; }
    public required string protocol { get; set; }
    /// <summary>
    /// 此入站连接的标识 唯一
    /// </summary>
    public required string tag { get; set; }
    public InboundSettings settings { get; set; } = new();
    public object? streamSettings { get; set; }
    /// <summary>
    /// 尝试探测流量的类型
    /// </summary>
    public InboundSniffing sniffing { get; set; }=new();
    /// <summary>
    /// 端口分配设置
    /// </summary>
    public InboundAllocate? allocate { get; set; }
}

public class InboundSettings
{
    public string auth { get; set; } = "noauth";
    public bool udp { get; set; } = true;
    public bool allowTransparent { get; set; }
}

public class InboundSniffing
{
    public bool enabled { get; set; } = true;
    public List<string>? destOverride { get; set; } = ["http", "tls"];
    public bool routeOnly { get; set; }
    public List<string>? domainsExcluded { get; set; }
    public bool metadataOnly { get; set; }
}

public class InboundAllocate
{
    public string? strategy { get; set; }
    public int refresh { get; set; }
    public int concurrency { get; set; }
}