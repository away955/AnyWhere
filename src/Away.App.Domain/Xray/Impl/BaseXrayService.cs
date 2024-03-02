using System.Diagnostics;

namespace Away.Domain.Xray.Impl;

public abstract class BaseXrayService : IBaseXrayService
{
    protected const string ExeFileName = "v2ray";
    protected readonly string _xrayConfigPath;
    protected readonly string _xrayPath;

    public BaseXrayService(string ConfigFileName)
    {
        Config = XrayConfig.Default;
        var baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
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
    public bool IsEnable { get; private set; }

    public void SaveConfig()
    {
        SetConfig(Config);
    }

    public void SetConfig(XrayConfig xrayConfig)
    {
        File.WriteAllText(_xrayConfigPath, JsonUtils.Serialize(xrayConfig));
    }

    public XrayConfig GetConfig()
    {
        var json = File.ReadAllText(_xrayConfigPath);
        Config = JsonUtils.Deserialize<XrayConfig>(json) ?? new XrayConfig();
        return Config;
    }

    private Action? XrayStop;
    public bool XrayStart()
    {
        if (IsEnable)
        {
            return true;
        }

        Log.Information("启动代理");
        Process xrayProcess = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _xrayPath,
                Arguments = $"run -c {_xrayConfigPath}",
            }
        };
        xrayProcess.OutputDataReceived += (sender, e) =>
        {
            if (string.IsNullOrWhiteSpace(e.Data))
            {
                return;
            }
            Log.Information(e.Data);
        };
        IsEnable = xrayProcess.Start();
        XrayStop = xrayProcess.Kill;
        return IsEnable;
    }

    public bool XrayClose()
    {
        if (!IsEnable)
        {
            return true;
        }
        Log.Information("关闭代理");
        XrayStop?.Invoke();
        IsEnable = false;
        return true;
    }

    public void XrayRestart()
    {
        XrayClose();
        XrayStart();
    }
}