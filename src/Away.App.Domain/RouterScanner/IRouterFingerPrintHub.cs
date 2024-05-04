using Away.App.Domain.RouterScanner.Impl;

namespace Away.App.Domain.RouterScanner;

/// <summary>
/// 路由器指纹库
/// </summary>
public interface IRouterFingerPrintHub
{
    /// <summary>
    /// 路由指纹
    /// </summary>
    public List<RouterFingerPrintMatch> Matches { get; }
}
