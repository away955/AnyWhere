namespace Away.App.Views;

[View("xray-setting-dns")]
public partial class XrayDnsView : UserControl, IView
{
    public XrayDnsView()
    {
        DataContext = AwayLocator.GetViewModel<XrayDnsViewModel>();
        InitializeComponent();
    }
}