namespace Away.App.Views;

public partial class TopHeader : UserControl
{
    public TopHeader()
    {
        this.DataContext = AwayLocator.GetViewModel<TopHeaderViewModel>();
        InitializeComponent();
    }
}