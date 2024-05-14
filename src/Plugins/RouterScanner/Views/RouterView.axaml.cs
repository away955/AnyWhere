namespace RouterScanner.Views;

public partial class RouterView : ReactiveUserControl<RouterViewModel>, IView
{
    public RouterView()
    {
        ViewModel = AwayLocator.GetService<RouterViewModel>();
        InitializeComponent();
    }
}