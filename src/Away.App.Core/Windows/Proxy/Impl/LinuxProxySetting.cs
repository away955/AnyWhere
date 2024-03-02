namespace Away.App.Core.Windows.Proxy.Impl;

public sealed class LinuxProxySetting : IProxySetting
{
    public string ProxyServer { get; set; } = string.Empty;
    public string ProxyOverride { get; set; } = string.Empty;
    public bool ProxyEnable { get; set; }

    public void GetProxy()
    { 
    }

    public bool Save()
    {
        return true;
    }
}
