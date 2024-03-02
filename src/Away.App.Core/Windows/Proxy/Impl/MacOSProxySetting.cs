namespace Away.App.Core.Windows.Proxy.Impl;

public sealed class MacOSProxySetting : IProxySetting
{
    public string ProxyServer { get; set; } = string.Empty;
    public string ProxyOverride { get; set; } = string.Empty;
    public bool ProxyEnable { get; set; }

    public void GetProxy()
    {
    }

    public bool Save()
    {
        return false;
    }
}
