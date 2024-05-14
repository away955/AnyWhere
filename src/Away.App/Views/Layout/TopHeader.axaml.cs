namespace Away.App.Views;

public partial class TopHeader : ReactiveUserControl<TopHeaderViewModel>
{
    public TopHeader()
    {
        ViewModel = AwayLocator.GetService<TopHeaderViewModel>();
        InitializeComponent();
    }
}