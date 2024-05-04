using Away.App.Domain.RouterScanner.Impl;

namespace Away.App.Domain.RouterScanner;

/// <summary>
/// 路由器漏洞扫描
/// </summary>
public interface IRouterVulScanner
{
    /// <summary>
    /// 扫描完成事件
    /// </summary>
    public event Action<VulResult>? OnCompleted;
    /// <summary>
    /// 代理
    /// </summary>
    public IWebProxy? Proxy { get; set; }
    /// <summary>
    /// 线程数
    /// </summary>
    public int Threads { get; set; }
    /// <summary>
    /// 超时时间/毫秒
    /// </summary>
    public int Timeout { get; set; }
    /// <summary>
    /// 路由器地址
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// 路由器信息
    /// </summary>
    public RouterVersionInfo RouterInfo { get; set; }
    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="cancellationToken"></param>
    void Run(CancellationToken cancellationToken = default);
}
