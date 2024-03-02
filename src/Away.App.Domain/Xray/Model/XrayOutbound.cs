namespace Away.Domain.Xray.Model;

/// <summary>
/// 出站连接配置
/// </summary>
public class XrayOutbound
{
    /// <summary>
    /// 用于发送数据的 IP 地址，当主机有多个 IP 地址时有效，默认值为 "0.0.0.0"
    /// </summary>
    public string? sendThrough { get; set; }
    public required string protocol { get; set; }
    /// <summary>
    /// 此出站连接的标识 唯一
    /// </summary>
    public required string tag { get; set; }

    public Dictionary<string, object>? settings { get; set; }
    public OutboundStreamSettings? streamSettings { get; set; }
    public OutboundProxySettings? proxySettings { get; set; }
    /// <summary>
    /// Mux 功能是在一条 TCP 连接上分发多个 TCP 连接的数据
    /// </summary>
    public OutboundMux mux { get; set; } = new();
}


public class OutboundUser
{
    public string? id { get; set; }
    public string? alterId { get; set; }
    public string? email { get; set; }
    public string? security { get; set; }
}

public class OutboundStreamSettings
{
    public string? network { get; set; }
    public string? security { get; set; }
    public TLSModel? tlsSettings { get; set; }
    public object? tcpSettings { get; set; }
    public KcpModel? kcpSettings { get; set; }
    public WebSocketModel? wsSettings { get; set; }
    public HttpModel? httpSettings { get; set; }
    public QuicModel? quicSettings { get; set; }
    public DomainSocketModel? dsSettings { get; set; }
    public GrpcModel? grpcSettings { get; set; }
    public Sockopt? sockopt { get; set; }
}

public class TLSModel
{
    public string? serverName { get; set; }
    public bool rejectUnknownSni { get; set; }
    public bool allowInsecure { get; set; }
    public List<string>? alpn { get; set; }
    public string? minVersion { get; set; }
    public string? maxVersion { get; set; }
    public string? cipherSuites { get; set; }
    public Certificates? certificates { get; set; }
    public bool disableSystemRoot { get; set; }
    public bool enableSessionResumption { get; set; }
    public string? fingerprint { get; set; }
    public List<string>? pinnedPeerCertificateChainSha256 { get; set; }
    public string? masterKeyLog { get; set; }
}

public class Certificates
{
    public int ocspStapling { get; set; }
    public bool oneTimeLoading { get; set; }
    public string? usage { get; set; }
    public string? certificateFile { get; set; }
    public string? keyFile { get; set; }
    public List<string>? certificate { get; set; }
    public List<string>? key { get; set; }
}

public class Sockopt
{
    public int mark { get; set; }
    public int tcpMaxSeg { get; set; }
    public bool tcpFastOpen { get; set; }
    public string? tproxy { get; set; }
    public string? domainStrategy { get; set; }
    public string? dialerProxy { get; set; }
    public bool acceptProxyProtocol { get; set; }
    public int tcpKeepAliveInterval { get; set; }
    public int tcpKeepAliveIdle { get; set; }
    public int tcpUserTimeout { get; set; }
    public string? tcpcongestion { get; set; }
    public string? @interface { get; set; }
    public bool V6Only { get; set; }
    public int tcpWindowClamp { get; set; }
    public bool tcpMptcp { get; set; }
    public bool tcpNoDelay { get; set; }
}

public class OutboundProxySettings
{
    public string? tag { get; set; }
}

public class OutboundMux
{
    /// <summary>
    /// 是否启用 Mux 转发请求
    /// </summary>
    public bool enabled { get; set; }

    /// <summary>
    /// 最大并发连接数。最小值 1，最大值 1024，默认值 8。
    /// 填负数，如 -1，不加载 mux 模块（v4.22.0+）
    /// </summary>
    public int concurrency { get; set; } = -1;
}