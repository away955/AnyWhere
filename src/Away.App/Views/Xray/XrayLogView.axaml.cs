namespace Away.App.Views;

[View("xray-setting-log")]
public partial class XrayLogView : UserControl, IView
{
    public XrayLogView()
    {
        DataContext = AwayLocator.GetViewModel<XrayLogViewModel>();
        InitializeComponent();
    }
}