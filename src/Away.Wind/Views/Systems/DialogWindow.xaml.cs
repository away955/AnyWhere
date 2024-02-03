using System.Windows.Input;

namespace Away.Wind.Views;

[DialogWindow]
public partial class DialogWindow : Window, IDialogWindow
{
    public DialogWindow()
    {
        InitializeComponent();
    }

    public IDialogResult Result { get; set; } = null!;

    private void Header_MouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            DragMove();
        }
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
