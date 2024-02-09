using Away.Service.Proxy;
using System.Diagnostics;

namespace Away.Service.Xray.Impl;

[ServiceInject(ServiceLifetime.Singleton)]
public class XrayService : BaseXrayService, IXrayService
{
    private readonly IProxySetting _proxySetting;
    public XrayService(IProxySetting proxySetting) : base("config.json")
    {
        _proxySetting = proxySetting;
        IsEnableGlobalProxy = _proxySetting.ProxyEnable;
    }

    public bool IsEnableGlobalProxy { get; private set; }

    public bool OpenGlobalProxy()
    {
        if (IsEnableGlobalProxy)
        {
            return true;
        }
        var inbound = Config.inbounds.FirstOrDefault();
        if (inbound == null)
        {
            return false;
        }
        _proxySetting.ProxyServer = $"{inbound.listen}:{inbound.port}";
        _proxySetting.ProxyEnable = true;
        _proxySetting.Save();
        IsEnableGlobalProxy = true;
        return true;
    }
    public bool CloseGlobalProxy()
    {
        if (!IsEnableGlobalProxy)
        {
            return true;
        }
        _proxySetting.ProxyEnable = false;
        _proxySetting.Save();
        IsEnableGlobalProxy = false;
        return true;
    }

    public void CloseAll()
    {
        CloseGlobalProxy();
        var xrays = Process.GetProcessesByName(ExeFileName);
        foreach (var xray in xrays)
        {
            xray.Kill();
        }
    }
}
