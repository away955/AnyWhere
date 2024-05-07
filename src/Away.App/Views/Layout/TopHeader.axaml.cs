namespace Away.App.Views;

public partial class TopHeader : UserControl
{
    public TopHeader()
    {
        this.DataContext = AwayLocator.GetService<TopHeaderViewModel>();
        InitializeComponent();
    }
}