namespace Away.Wind.Views;

[DialogWindow]
public partial class DialogWindow : Window, IDialogWindow
{
    public DialogWindow()
    {
        InitializeComponent();
    }

    public IDialogResult Result { get; set; }
}
