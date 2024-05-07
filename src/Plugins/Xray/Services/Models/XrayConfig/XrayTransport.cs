namespace Xray.Services.Models;

/// <summary>
/// 用于配置 Xray 其它服务器建立和使用网络连接的方式
/// </summary>
public sealed class XrayTransport
{
    public TcpModel? tcpSettings { get; set; }
    public KcpModel? kcpSettings { get; set; }
    public WebSocketModel? wsSettings { get; set; }
    public HttpModel? httpSettings { get; set; }
    public QuicModel? quicSettings { get; set; }
    public DomainSocketModel? dsSettings { get; set; }
    public GrpcModel? grpcSettings { get; set; }
}

/// <summary>
/// 针对 QUIC 连接的配置
/// </summary>
public sealed class QuicModel
{
    public string? security { get; set; }
    public string? key { get; set; }
    public Dictionary<string, object>? header { get; set; }
}

/// <summary>
/// 针对 Domain Socket 连接的配置
/// </summary>
public sealed class DomainSocketModel
{
    public string? path { get; set; }
    public bool @abstract { get; set; }
    public bool padding { get; set; }
}

/// <summary>
/// 针对 gRPC 连接的配置
/// </summary>
public sealed class GrpcModel
{
    public string? serviceName { get; set; }
    public bool multiMode { get; set; }
    public string? user_agent { get; set; }
    public int idle_timeout { get; set; }
    public int health_check_timeout { get; set; }
    public bool permit_without_stream { get; set; }
    public int initial_windows_size { get; set; }
}

/// <summary>
/// 针对 WebSocket 连接的配置。
/// </summary>
public sealed class HttpModel
{
    public List<string>? host { get; set; }
    public string? path { get; set; }
    public int read_idle_timeout { get; set; }
    public int health_check_timeout { get; set; }
    public string? method { get; set; }
    public Dictionary<string, List<string>>? headers { get; set; }
}

/// <summary>
/// 针对 mKCP 连接的配置。
/// </summary>
public sealed class WebSocketModel
{
    public bool acceptProxyProtocol { get; set; }
    public string? path { get; set; }
    public Dictionary<string, object>? headers { get; set; }
}

/// <summary>
/// 对应传输配置的 kcpSettings 项
/// </summary>
public sealed class KcpModel
{
    public int mtu { get; set; }
    public int tti { get; set; }
    public int uplinkCapacity { get; set; }
    public int downlinkCapacity { get; set; }
    public bool congestion { get; set; }
    public int readBufferSize { get; set; }
    public int writeBufferSize { get; set; }
    public Dictionary<string, object>? header { get; set; }
    public string? seed { get; set; }
}

/// <summary>
/// TCP 传输模式
/// </summary>
public sealed class TcpModel
{
    /// <summary>
    /// 仅用于 inbound，指示是否接收 PROXY protocol
    /// </summary>
    public bool acceptProxyProtocol { get; set; }
    public Dictionary<string, object>? header { get; set; }
}