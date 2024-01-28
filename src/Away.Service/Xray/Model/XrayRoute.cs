namespace Away.Service.Xray.Model;

/// <summary>
/// 路由功能。可以设置规则分流数据从不同的 outbound 发出.
/// </summary>
public class XrayRoute
{
    public string? domainStrategy { get; set; }
    public string? domainMatcher { get; set; }
    public List<RouteRule>? rules { get; set; }
    public List<RouteBalancer>? balancers { get; set; }
}

public class RouteRule
{
    public string? domainMatcher { get; set; }
    public string? type { get; set; }
    public List<string>? domain { get; set; }
    public List<string>? ip { get; set; }
    public string? port { get; set; }
    public string? sourcePort { get; set; }
    public string? network { get; set; }
    public List<string>? source { get; set; }
    public List<string>? user { get; set; }
    public List<string>? inboundTag { get; set; }
    public List<string>? protocol { get; set; }
    public Dictionary<string, object>? attrs { get; set; }
    public string? outboundTag { get; set; }
    public string? balancerTag { get; set; }
}

public class RouteBalancer
{
    public string? tag { get; set; }
    public List<string>? selector { get; set; }
    public Dictionary<string, string>? strategy { get; set; }
}