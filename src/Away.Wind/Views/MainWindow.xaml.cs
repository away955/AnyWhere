using System.Windows.Input;

namespace Away.Wind.Views;

[Navigation]
[ViewModel(typeof(MainWindowViewModel))]
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void TopHeader_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }
}