namespace RouterScanner.Services;

/// <summary>
/// 路由器扫描器
/// </summary>
public interface IRouterScanner
{
    /// <summary>
    /// 扫描指纹完成事件
    /// </summary>
    public event Action<FingerPrintResult>? OnFingerPrintCompleted;
    /// <summary>
    /// 扫描漏洞完成事件
    /// </summary>
    public event Action<VulResult>? OnVulCompleted;
    /// <summary>
    /// 扫描IP<br/>
    /// <code>
    /// 192.168.1.1
    /// 192.168.1.1-192.168.1.255
    /// </code>
    /// </summary>
    public string IPs { get; set; }
    /// <summary>
    /// 扫描的端口号<br/>
    /// <code>
    /// 80
    /// 80-90
    /// </code>
    /// </summary>
    public string Ports { get; set; }
    /// <summary>
    /// 指纹扫描线程数
    /// </summary>
    public int FingerPrintThreads { get; set; }
    /// <summary>
    /// 指纹扫描超时时间/毫秒
    /// </summary>
    public int FingerPrintTimeout { get; set; }
    /// <summary>
    /// 漏洞扫描线程数
    /// </summary>
    public int VulThreads { get; set; }
    /// <summary>
    /// 漏洞扫描超时时间/毫秒
    /// </summary>
    public int VulTimeout { get; set; }
    /// <summary>
    /// 网络代理
    /// </summary>
    public IWebProxy? Proxy { get; set; }

    /// <summary>
    /// 取消
    /// </summary>
    void Cancel();
    /// <summary>
    /// 执行
    /// </summary>
    void Run();
}
