using Away.Service.Xray.Model;

namespace Away.Wind.Views.Xray;

public class XrayOutboundEditVM : BindableBase, IDialogAware
{
    public XrayOutboundEditVM()
    {

    }
    public string Title => "出站配置";

    public event Action<IDialogResult> RequestClose = null!;

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        var parameters = new DialogParameters();
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }


    private XrayOutbound _outbound = new();
    /// <summary>
    /// 出站连接配置
    /// </summary>
    public XrayOutbound Outbound { get => _outbound; set => SetProperty(ref _outbound, value); }
}
