namespace Xray.Views;

public partial class XrayDnsView : ReactiveUserControl<XrayDnsViewModel>, IView
{
    public XrayDnsView()
    {
        ViewModel = AwayLocator.GetService<XrayDnsViewModel>();
        InitializeComponent();
    }
}