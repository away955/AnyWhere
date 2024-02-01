namespace Away.Wind.Views.Xray;

public class XrayOutboundEditDialogViewModel : BindableBase, IDialogAware
{
    private IDialogParameters? _parameters;
    public XrayOutboundEditDialogViewModel()
    {

    }
    public string Title => "出站配置";

    public event Action<IDialogResult> RequestClose;

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
        _parameters = parameters;
    }
}
