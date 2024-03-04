namespace Away.Domain.Xray.Model;

/// <summary>
/// 路由功能。可以设置规则分流数据从不同的 outbound 发出.
/// </summary>
public class XrayRoute
{
    /// <summary>
    /// 域名解析策略。
    /// AsIs：只使用域名进行路由选择，默认值；
    /// IPIfNonMatch：当域名没有匹配任何基于域名的规则时，将域名解析成 IP（A 记录或 AAAA 记录），进行基于 IP 规则的匹配；
    /// 当一个域名有多个 IP 地址时，会尝试匹配所有的 IP 地址，直到其中一个与某个 IP 规则匹配为止；
    /// 解析后的 IP 仅在路由选择时起作用，转发的数据包中依然使用原始域名。
    /// IPOnDemand：当匹配时碰到任何基于 IP 的规则，立即将域名解析为 IP 进行匹配。
    /// </summary>
    public string domainStrategy { get; set; } = "AsIs";
    /// <summary>
    /// 选择要使用的域名匹配算法。
    /// linear：使用线性匹配算法，默认值；
    /// mph：使用最小完美散列（minimal perfect hash）算法（v4.36.1+）
    /// </summary>
    public string domainMatcher { get; set; } = "linear";
    /// <summary>
    /// 路由规则
    /// </summary>
    public List<RouteRule> rules { get; set; } = [];
    /// <summary>
    /// 负载均衡
    /// </summary>
    public List<RouteBalancer>? balancers { get; set; }

    public static XrayRoute Default
    {
        get
        {
            return new XrayRoute
            {
                rules = [
                    new RouteRule
                    {
                        type = "field",
                        outboundTag = "block",
                        domain = ["geosite:category-ads-all"]
                    },
                    new RouteRule
                    {
                        type = "field",
                        outboundTag = "direct",
                        domain = ["geosite:cn"]
                    },
                    new RouteRule
                    {
                        type = "field",
                        outboundTag = "direct",
                        ip = ["geoip:private", "geoip:cn"]
                    }
                ]
            };
        }
    }
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