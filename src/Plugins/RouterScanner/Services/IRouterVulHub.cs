namespace RouterScanner.Services;

/// <summary>
/// 路由器漏洞库
/// </summary>
public interface IRouterVulHub
{
    /// <summary>
    /// 漏洞编号
    /// </summary>
    public string CVE { get; }
    /// <summary>
    /// 漏洞类型
    /// </summary>
    public string VulType { get; }
    /// <summary>
    /// 厂商|路由名称
    /// </summary>
    public string Production { get; }
    /// <summary>
    /// 受影响的版本
    /// </summary>
    public List<string> Version { get; }
    /// <summary>
    /// 路由地址
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// 攻击参数
    /// </summary>
    public Payload Payload { get; set; }
    /// <summary>
    /// 网络代理
    /// </summary>
    public IWebProxy? Proxy { get; set; }
    /// <summary>
    /// 漏洞验证
    /// </summary>
    /// <returns></returns>
    ValueTask<VulResult> Poc(CancellationToken cancellationToken = default);
    /// <summary>
    /// 漏洞利用
    /// </summary>
    /// <returns></returns>
    ValueTask<VulResult> Exp(CancellationToken cancellationToken = default);
}

public sealed class VulResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public required IRouterVulHub Vul { get; set; }
}


public sealed class Payload
{
    /// <summary>
    /// 命令
    /// </summary>
    public string Cmd { get; set; } = string.Empty;
    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
