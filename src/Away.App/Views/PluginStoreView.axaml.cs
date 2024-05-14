namespace Away.App.Views;

public partial class PluginStoreView : ReactiveUserControl<PluginStoreViewModel>, IView
{
    public PluginStoreView()
    {
        ViewModel = AwayLocator.GetService<PluginStoreViewModel>();
        InitializeComponent();
    }
}