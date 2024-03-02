namespace Away.Domain.Xray.Model;

/// <summary>
/// 本地策略，可以设置不同的用户等级和对应的策略设置。
/// </summary>
public class XrayPolicy
{
    public Dictionary<string, PolicyLevel>? levels { get; set; }
    public PolicySystem? system { get; set;}
}

public class PolicyLevel
{
    public int handshake { get; set; }
    public int connIdle { get; set; }
    public int uplinkOnly { get; set; }
    public int downlinkOnly { get; set; }
    public string? statsUserUplink { get; set; }
    public string? statsUserDownlink { get; set; }
    public int bufferSize { get; set; }
}

public class PolicySystem
{
    public string? statsInboundUplink { get; set; }
    public string? statsInboundDownlink { get; set; }
    public string? statsOutboundUplink { get; set; }
    public string? statsOutboundDownlink { get; set; }
}