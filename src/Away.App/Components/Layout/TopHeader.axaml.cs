namespace Away.App.Components.Layout;

public partial class TopHeader : UserControl
{
    public TopHeader()
    {
        this.DataContext = App.Current?.Services.GetViewModel<TopHeaderViewModel>();
        InitializeComponent();
    }
}