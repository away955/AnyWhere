namespace Away.Service.Proxy;

/// <summary>
/// 系统代理配置
/// </summary>
public interface IProxySetting
{
    string ProxyServer { get; set; }
    string ProxyOverride { get; set; }
    bool ProxyEnable { get; set; }

    bool Save();
    void GetProxy();
}
