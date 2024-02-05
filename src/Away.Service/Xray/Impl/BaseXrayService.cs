using System.Diagnostics;

namespace Away.Service.Xray.Impl;

public abstract class BaseXrayService : IBaseXrayService
{
    protected const string ExeFileName = "v2ray";
    protected readonly string _xrayConfigPath;
    protected readonly string _xrayPath;


    public BaseXrayService(string ConfigFileName)
    {
        Config = XrayConfig.Default;


        var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xray");
        _xrayConfigPath = Path.Combine(baseDirectory, ConfigFileName);
        _xrayPath = Path.Combine(baseDirectory, ExeFileName);

        // 初始化配置文件
        if (!File.Exists(_xrayConfigPath))
        {
            SetConfig(Config);
        }
        GetConfig();

    }

    public XrayConfig Config { get; private set; }
    public bool IsOpened { get; private set; }

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

        Log.Information("启动代理");

        Process xrayProcess = new()
        {
            StartInfo = new ProcessStartInfo(_xrayPath, $"run -c {_xrayConfigPath}"),
        };
        xrayProcess.OutputDataReceived += (sender, e) =>
        {
            if (string.IsNullOrWhiteSpace(e.Data))
            {
                return;
            }
            Log.Information(e.Data);
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
        Log.Information("关闭代理");
        XrayStop?.Invoke();
        IsOpened = false;
    }

    public void XrayRestart()
    {
        XrayClose();
        XrayStart();
    }
}