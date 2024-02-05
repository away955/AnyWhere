using System.Diagnostics;

namespace Away.Service.Xray.Impl;

[ServiceInject(ServiceLifetime.Singleton)]
public class XrayService(ILogger<XrayService> logger) : BaseXrayService(logger, ExeFileName, "config.json"), IXrayService
{
    private const string ExeFileName = "v2ray";

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
