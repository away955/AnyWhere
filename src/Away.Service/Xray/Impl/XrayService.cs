using System.Diagnostics;

namespace Away.Service.Xray.Impl;

[ServiceInject(ServiceLifetime.Singleton)]
public class XrayService : IXrayService
{
    private readonly ILogger<XrayService> _logger;
    private readonly string _xrayConfigPath;
    private readonly string _xrayPath;
    public XrayConfig Config { get; private set; }

    public bool IsOpened { get; private set; }

    public XrayService(ILogger<XrayService> logger)
    {
        _logger = logger;
        Config = new XrayConfig();


        var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xray");
        _xrayConfigPath = Path.Combine(baseDirectory, "config.json");
        _xrayPath = Path.Combine(baseDirectory, "xray");

        // 初始化配置文件
        if (!File.Exists(_xrayConfigPath))
        {
            SetConfig(Config);
        }
        GetConfig();

    }

    public void SaveConfig()
    {
        SetConfig(Config);
    }

    public void SetConfig(XrayConfig xrayConfig)
    {
        File.WriteAllText(_xrayConfigPath, XrayUtils.Serialize(xrayConfig));
    }

    public XrayConfig GetConfig()
    {
        var json = File.ReadAllText(_xrayConfigPath);
        Config = XrayUtils.Deserialize<XrayConfig>(json) ?? new XrayConfig();
        return Config;
    }

    private Action? XrayStop;
    public void XrayStart()
    {
        if (IsOpened)
        {
            return;
        }

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
        IsOpened = true;
        XrayStop = xrayProcess.Kill;
    }

    public void XrayClose()
    {
        if (!IsOpened)
        {
            return;
        }
        _logger.LogInformation("Xray Close");
        XrayStop?.Invoke();
        IsOpened = false;
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
