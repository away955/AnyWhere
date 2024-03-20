namespace Away.App.Views;

[View("xray-setting-log")]
public partial class XrayLogView : ReactiveUserControl<XrayLogViewModel>, IView
{
    public XrayLogView()
    {
        ViewModel = AwayLocator.GetViewModel<XrayLogViewModel>();
        InitializeComponent();
    }
}