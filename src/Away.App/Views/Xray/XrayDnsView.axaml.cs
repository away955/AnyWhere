namespace Away.App.Views;

[View("xray-setting-dns")]
public partial class XrayDnsView : ReactiveUserControl<XrayDnsViewModel>, IView
{
    public XrayDnsView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayDnsViewModel>();
        InitializeComponent();
    }
}