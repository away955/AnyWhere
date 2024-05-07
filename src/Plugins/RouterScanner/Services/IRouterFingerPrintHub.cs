using RouterScanner.Services.Impl;

namespace RouterScanner.Services;

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
