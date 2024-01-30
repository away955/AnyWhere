using Away.Service.Utils;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Away.Service.Xray.Impl;

[ServiceInject]
public class XrayService : IXrayService
{
    private readonly ILogger<XrayService> _logger;
    private readonly string _xrayConfigPath;
    private readonly string _xrayPath;
    private XrayConfig? _xrayConfig;

    public XrayService(ILogger<XrayService> logger)
    {
        _logger = logger;
        _xrayConfig = new XrayConfig();


        var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xray");
        _xrayConfigPath = Path.Combine(baseDirectory, "config.json");
        _xrayPath = Path.Combine(baseDirectory, "xray");

        // 初始化配置文件
        if (!File.Exists(_xrayConfigPath))
        {
            SetConfig(_xrayConfig);
        }
        GetConfig();

    }

    public void SetConfig(XrayConfig xrayConfig)
    {
        File.WriteAllText(_xrayConfigPath, XrayUtils.Serialize(xrayConfig));
    }

    public XrayConfig? GetConfig()
    {
        var json = File.ReadAllText(_xrayConfigPath);
        _xrayConfig = XrayUtils.Deserialize<XrayConfig>(json);
        return _xrayConfig;
    }

    private Action? XrayStop;
    public void XrayStart()
    {
        _logger.LogInformation("Xray Start");

        Process xrayProcess = new()
        {
            StartInfo = new ProcessStartInfo(_xrayPath),
        };
        xrayProcess.OutputDataReceived += (sender, e) =>
        {
            _logger.LogInformation("{}", e.Data);
        };
        xrayProcess.Start();
        XrayStop = xrayProcess.Kill;
    }

    public void XrayClose()
    {
        _logger.LogInformation("Xray Close");
        XrayStop?.Invoke();
    }

    public static void XraysClose()
    {
        Log.Logger.Information("All Xray Close");
        var xrays = Process.GetProcessesByName("xray");
        foreach (var xray in xrays)
        {
            xray.Kill();
        }
    }
}
