using System.Windows;

namespace Window2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CloseApp_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}