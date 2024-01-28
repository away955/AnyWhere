namespace Away.Service.Xray.Model;

/// <summary>
/// 入站连接配置
/// </summary>
public class XrayInbound
{
    public string? listen { get; set; }
    public int port { get; set; }
    public string? protocol { get; set; }
    public string? tag { get; set; }
    public InboundSettings? settings { get; set; }
    public object? streamSettings { get; set; }
    public InboundSniffing? sniffing { get; set; }
    public InboundAllocate? allocate { get; set; }
}

public class InboundSettings
{
    public string? auth { get; set; }
    public bool udp { get; set; }
    public bool allowTransparent { get; set; }
}

public class InboundSniffing
{
    public bool enabled { get; set; }
    public List<string>? destOverride { get; set; }
    public bool routeOnly { get; set; }
    public List<string>? domainsExcluded { get; set; }
    public bool metadataOnly {  get; set; } 
}

public class InboundAllocate
{
    public string? strategy { get; set; }
    public int refresh { get; set; }
    public int concurrency { get; set; }
}