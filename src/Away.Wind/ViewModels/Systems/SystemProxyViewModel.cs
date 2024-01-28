using Away.Service.Proxy;

namespace Away.Wind.ViewModels;

/// <summary>
/// 系统代理
/// </summary>
public class SystemProxyViewModel : BindableBase
{
    private readonly IProxySetting _proxySetting;
    public SystemProxyViewModel(IProxySetting proxySetting)
    {
        _proxySetting = proxySetting;

        _server = _proxySetting.ProxyServer;
        _whiteList = _proxySetting.ProxyOverride;
        _isEnable = _proxySetting.ProxyEnable;

        SaveCommand = new DelegateCommand(OnSaveCommand);
    }

    private string _server;
    public string Server
    {
        get => _server;
        set => SetProperty(ref _server, value);
    }

    private string _whiteList = "<local>";
    public string WhiteList
    {
        get => _whiteList;
        set => SetProperty(ref _whiteList, value);
    }

    private bool _isEnable;
    public bool IsEnable
    {
        get => _isEnable;
        set => SetProperty(ref _isEnable, value);
    }

    public DelegateCommand SaveCommand { get; private set; }
    public void OnSaveCommand()
    {
        _proxySetting.ProxyServer = _server;
        _proxySetting.ProxyOverride = _whiteList;
        _proxySetting.ProxyEnable = _isEnable;
        _proxySetting.SetProxy();
    }
}
