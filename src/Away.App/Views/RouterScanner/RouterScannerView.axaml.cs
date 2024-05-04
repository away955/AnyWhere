namespace Away.App.Views;

[View("router-scanner")]
public partial class RouterScannerView : ReactiveUserControl<RouterScannerViewModel>, IView
{
    public RouterScannerView()
    {
        ViewModel = AwayLocator.GetViewModel<RouterScannerViewModel>();
        InitializeComponent();
    }
}