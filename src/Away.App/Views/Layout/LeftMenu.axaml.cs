namespace Away.App.Views;

public partial class LeftMenu : ReactiveUserControl<LeftMenuViewModel>
{
    public LeftMenu()
    {
        ViewModel = AwayLocator.GetService<LeftMenuViewModel>();
        InitializeComponent();
    }
}