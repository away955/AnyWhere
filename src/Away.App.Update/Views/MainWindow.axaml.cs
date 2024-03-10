namespace Away.App.Update.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
        InitializeComponent();
        MessageBusListen();
    }

    public void TopHeader_PointerMoved(object sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }

    private void MessageBusListen()
    {
        // ´°¿Ú×´Ì¬ÇÐ»»
        MessageBus.Current.Listen<string>().Subscribe(cmd =>
        {
            switch (cmd)
            {
                case "Minimized":
                    WindowState = WindowState.Minimized;
                    break;
                case "Close":
                    Close();
                    break;
            }
        });
    }
}