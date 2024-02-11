namespace Away.Service.Xray;

/// <summary>
/// Xray 的配置文件
/// </summary>
public sealed class XrayConfig
{
    /// <summary>
    /// 日志配置，控制 Xray 输出日志的方式.
    /// </summary>
    public XrayLog? log { get; set; }
    /// <summary>
    /// 提供了一些 API 接口供远程调用
    /// </summary>
    public XrayApi? api { get; set; }
    /// <summary>
    /// 内置的 DNS 服务器. 如果没有配置此项，则使用系统的 DNS 设置。
    /// </summary>
    public XrayDns dns { get; set; } = XrayDns.Default;
    /// <summary>
    /// 路由功能。可以设置规则分流数据从不同的 outbound 发出.
    /// </summary>
    public XrayRoute routing { get; set; } = XrayRoute.Default;
    /// <summary>
    /// 本地策略，可以设置不同的用户等级和对应的策略设置。
    /// </summary>
    public XrayPolicy? policy { get; set; }
    /// <summary>
    /// 一个数组，每个元素是一个入站连接配置。
    /// </summary>
    public List<XrayInbound> inbounds { get; set; } = [];
    /// <summary>
    /// 一个数组，每个元素是一个出站连接配置。
    /// </summary>
    public List<XrayOutbound> outbounds { get; set; } = [];
    /// <summary>
    /// 用于配置 Xray 其它服务器建立和使用网络连接的方式
    /// </summary>
    public XrayTransport? transport { get; set; }
    /// <summary>
    /// 用于配置流量数据的统计
    /// </summary>
    public XrayStats? stats { get; set; }
    /// <summary>
    /// 反向代理。可以把服务器端的流量向客户端转发，即逆向流量转发。
    /// </summary>
    public XrayReverse? reverse { get; set; }
    /// <summary>
    /// FakeDNS 配置。可配合透明代理使用，以获取实际域名。
    /// </summary>
    public List<XrayFakedns>? fakedns { get; set; }
    /// <summary>
    /// metrics 配置。更直接（希望更好）的统计导出方式。
    /// </summary>
    public XrayMetrics? metrics { get; set; }


    public static XrayConfig Default { get => CreateDefault(); }
    private static XrayConfig CreateDefault()
    {
        XrayConfig config = new();
        config.log = XrayLog.Default;
        config.api = XrayApi.Default;
        config.inbounds = [
            new XrayInbound
            {
                listen = "127.0.0.1",
                port = 1080,
                protocol = "http",
                tag = "http",
            }
        ];

        config.outbounds = [
            new XrayOutbound
            {
                tag = "direct",
                protocol = "freedom",
            },
            new XrayOutbound
            {
                tag = "block",
                protocol = "blackhole",
                settings = new Dictionary<string, object> {
                    { "response", new { type = "http" } }
                }
            },
        ];

        return config;
    }
}

