using CliWrap.EventStream;
using System.Diagnostics;

namespace Away.Domain.Xray.Impl;

public abstract partial class BaseXrayService : IBaseXrayService
{
    protected const string ExeFileName = "v2ray";
    protected readonly string _xrayConfigPath;
    protected readonly string _xrayPath;

    public XrayNodeEntity? CurrentNode { get; private set; }

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
    public bool IsEnable { get; protected set; }

    public void SaveConfig()
    {
        SetConfig(Config);
    }

    public void SetConfig(XrayConfig xrayConfig)
    {
        File.WriteAllText(_xrayConfigPath, JsonUtils.Serialize(xrayConfig));
    }

    public void SetNode(XrayNodeEntity node)
    {
        CurrentNode = node;
        Config.SetOutbound(node);
    }

    public XrayConfig GetConfig()
    {
        var json = File.ReadAllText(_xrayConfigPath);
        Config = JsonUtils.Deserialize<XrayConfig>(json) ?? new XrayConfig();
        return Config;
    }

    protected Action? XrayStop { get; set; }
    public bool XrayStart()
    {
        if (IsEnable)
        {
            return true;
        }

        Log.Information("启动代理");
        Task.Run(async () =>
        {
            var cli = CliWrap.Cli.Wrap(_xrayPath)
                .WithArguments($"run -c {_xrayConfigPath}");

            await foreach (var cmdEvent in cli.ListenAsync())
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        XrayStop = Process.GetProcessById(started.ProcessId).Kill;
                        break;
                    case StandardOutputCommandEvent stdOut:
                        OnMessage(stdOut.Text);
                        break;
                    case StandardErrorCommandEvent stdErr:
                        OnMessage(stdErr.Text);
                        break;
                    case ExitedCommandEvent exited:
                        break;
                }
            }
        });
        IsEnable = true;
        return IsEnable;
    }

    private void OnMessage(string msg)
    {
        Log.Information(V2rayLogRegex().Replace(msg, string.Empty).Trim());
        // v2ray 启动成功
        var regStartUp = V2rayStartedRegex().Match(msg);
        if (regStartUp.Success)
        {
            OnMessage(msg, V2rayState.Started);
        }
        // v2ray 启动失败
        var regStartFailed = V2rayFailedStartRegex().Match(msg);
        if (regStartFailed.Success)
        {
            OnMessage(msg, V2rayState.FailedStart);
        }
        // 网络异常
        if (msg.Contains("all retry attempts failed"))
        {
            OnMessage(msg, V2rayState.FailedRetry);
        }
    }

    protected virtual void OnMessage(string msg, V2rayState state)
    {
    }

    public virtual bool XrayClose()
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

    [GeneratedRegex(@"^\d{4}/\d{2}/\d{2} \d{2}:\d{2}:\d{2}")]
    private static partial Regex V2rayLogRegex();
    [GeneratedRegex("V2Ray.*.started")]
    private static partial Regex V2rayStartedRegex();
    [GeneratedRegex("Failed to start")]
    private static partial Regex V2rayFailedStartRegex();
}
