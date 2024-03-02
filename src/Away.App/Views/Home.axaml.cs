namespace Away.App;

[View("home")]
public partial class Home : UserControl, IView
{
    public Home()
    {
        DataContext = App.Current?.Services.GetViewModel<HomeViewModel>();
        InitializeComponent();
    }
}