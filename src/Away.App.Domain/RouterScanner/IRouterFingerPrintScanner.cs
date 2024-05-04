using Away.App.Domain.RouterScanner.Impl;

namespace Away.App.Domain.RouterScanner;

/// <summary>
/// 路由器指纹识别
/// </summary>
public interface IRouterFingerPrintScanner
{
    /// <summary>
    /// 网络代理
    /// </summary>
    public IWebProxy? Proxy { get; set; }
    /// <summary>
    /// 路由器地址
    /// </summary>
    public IPEndPoint Host { get; set; }
    /// <summary>
    /// 超时时间/毫秒
    /// </summary>
    public int Timeout { get; set; }
    /// <summary>
    /// 开始识别路由器
    /// </summary>
    /// <returns></returns>
    ValueTask<FingerPrintResult> Run(CancellationToken cancellationToken = default);
}


public sealed class FingerPrintResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public RouterVersionInfo? Result { get; set; }
    public static FingerPrintResult OK(bool success, string url, string message = "", RouterVersionInfo? result = null)
    {
        return new FingerPrintResult { Success = success, Url = url, Message = message, Result = result };
    }
}