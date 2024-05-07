namespace Xray.Services.Impl;

public sealed class XrayService : XrayServiceBase, IXrayService
{
    private const int FailedTotal = 3;
    private int _failedCount;
    public bool IsHealthCheck { get; set; }
    public bool IsEnableGlobalProxy { get; private set; }

    public event Action? OnChangeNode;

    private readonly IProxySetting _proxySetting;
    private readonly IXrayNodeRepository _nodeRepository;
    private readonly IXraySetting _xrayOptions;
    public XrayService(
        IProxySetting proxySetting,
        IXraySetting options,
        IXrayNodeRepository nodeRepository) : base("config.json")
    {
        _proxySetting = proxySetting;
        _xrayOptions = options;
        _nodeRepository = nodeRepository;
        IsEnableGlobalProxy = _proxySetting.ProxyEnable;
        var process = Process.GetProcessesByName(ExeFileName)?.FirstOrDefault();
        IsEnable = process != null;
        if (process != null)
        {
            XrayStop = process.Kill;
        }
    }

    private void SetXrayOptions()
    {
        var inbound = Config.inbounds.FirstOrDefault();
        if (inbound == null)
        {
            return;
        }
        _xrayOptions.SetHost(inbound.listen ?? "127.0.0.1", inbound.port);
    }

    public bool OpenGlobalProxy()
    {
        if (IsEnableGlobalProxy)
        {
            return true;
        }
        SetXrayOptions();

        _proxySetting.ProxyServer = _xrayOptions.Host;
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



    protected override void OnMessage(string msg, V2rayState state)
    {
        if (IsHealthCheck && state == V2rayState.FailedRetry && ++_failedCount == FailedTotal)
        {
            ChangedNode();
        }
    }

    private void ChangedNode()
    {
        Log.Information("正在自动切换节点...");
        XrayClose();
        if (CurrentNode != null)
        {
            Log.Information($"旧节点：{CurrentNode!.Host}:{CurrentNode.Port}");
            _nodeRepository.DeleteById(CurrentNode.Id);
        }

        var newNode = _nodeRepository.GetList().OrderByDescending(o => o.Speed).FirstOrDefault();
        if (newNode != null)
        {
            SetNode(newNode);
            _nodeRepository.SetChecked(newNode);
            Log.Information($"新节点：{newNode.Host}:{newNode.Port}");
            XrayStart();
            Log.Information($"自动切换节点成功");
            _failedCount = 0;
            OnChangeNode?.Invoke();
            return;
        }
    }
}
