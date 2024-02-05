using System.Diagnostics;

namespace Away.Service.Xray.Impl;

[ServiceInject(ServiceLifetime.Singleton)]
public class XrayService() : BaseXrayService("config.json"), IXrayService
{
    public static void XraysClose()
    {
        Log.Logger.Information("关闭所有代理");
        var xrays = Process.GetProcessesByName(ExeFileName);
        foreach (var xray in xrays)
        {
            xray.Kill();
        }
    }
}
