namespace Away.Domain.Xray;

public interface IXrayService : IXrayServiceBase
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
    /// <summary>
    /// 健康检查，自动切换节点
    /// </summary>
    public bool IsHealthCheck { get; set; }

    event Action? OnChangeNode;
}
