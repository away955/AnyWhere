namespace Xray.Services.Impl;

public class LinuxProxySetting : ProxySettingBase, IProxySetting
{
    public string ProxyServer { get; set; } = string.Empty;
    public string ProxyOverride { get; set; } = string.Empty;
    public bool ProxyEnable { get; set; }

    public void GetProxy()
    {
        (ProxyServer, ProxyServer) = Get();
    }

    public bool Save()
    {
        return Set(ProxyServer, ProxyOverride, ProxyEnable);
    }
}
