namespace Away.Service.Xray;

public interface IXrayService : IBaseXrayService
{
    /// <summary>
    /// 是否启动全局代理
    /// </summary>
    public bool IsEnableGlobalProxy { get; }
    /// <summary>
    /// 开启全局代理
    /// </summary>
    /// <returns></returns>
    bool OpenGlobalProxy();
    /// <summary>
    /// 关闭全局代理
    /// </summary>
    /// <returns></returns>
    bool CloseGlobalProxy();
    /// <summary>
    /// 关闭xray和全局代理
    /// </summary>
    void CloseAll();
}
