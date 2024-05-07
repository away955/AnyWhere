namespace Xray.Views;

public partial class XrayLogView : ReactiveUserControl<XrayLogViewModel>, IView
{
    public XrayLogView()
    {
        ViewModel = AwayLocator.GetService<XrayLogViewModel>();
        InitializeComponent();
    }
}