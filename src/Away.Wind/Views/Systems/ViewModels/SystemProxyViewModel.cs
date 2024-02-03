using Away.Service.Proxy;

namespace Away.Wind.Views.Systems;

/// <summary>
/// 系统代理
/// </summary>
public class SystemProxyViewModel : BindableBase, IDialogAware
{
    private readonly IProxySetting _proxySetting;
    public SystemProxyViewModel(IProxySetting proxySetting)
    {
        _proxySetting = proxySetting;

        _server = _proxySetting.ProxyServer;
        _whiteList = _proxySetting.ProxyOverride;
        _isEnable = _proxySetting.ProxyEnable;

        SaveCommand = new(OnSaveCommand);
    }

    #region 弹框

    public string Title => "系统代理";
    public event Action<IDialogResult> RequestClose = null!;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {

    }
    #endregion

    private string _server = string.Empty;
    /// <summary>
    /// 代理地址
    /// </summary>
    public string Server
    {
        get => _server;
        set => SetProperty(ref _server, value);
    }

    private string _whiteList = "<local>";
    /// <summary>
    /// 白名单
    /// </summary>
    public string WhiteList
    {
        get => _whiteList;
        set => SetProperty(ref _whiteList, value);
    }

    private bool _isEnable;
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnable
    {
        get => _isEnable;
        set => SetProperty(ref _isEnable, value);
    }

    /// <summary>
    /// 保存
    /// </summary>
    public DelegateCommand SaveCommand { get; private set; }
    public void OnSaveCommand()
    {
        _proxySetting.ProxyServer = _server;
        _proxySetting.ProxyOverride = _whiteList;
        _proxySetting.ProxyEnable = _isEnable;
        _proxySetting.SetProxy();

        RequestClose.Invoke(new DialogResult(ButtonResult.OK));
    }

}
