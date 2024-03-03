namespace Away.App.Views;

public partial class MainWindow : Window
{
    private readonly WindowNotificationManager? _nofityManager;
    public MainWindow()
    {
        this.DataContext = AwayLocator.GetViewModel<MainWindowViewModel>();
        this.InitializeComponent();
        _nofityManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 5
        };
        MessageBusListen();
    }

    private void ViewChange(string? url = null) 
    {
        var view = AwayLocator.GetView(url) ?? AwayLocator.GetView("404");
        this.MainBox.Content = view;
    }

    public void TopHeader_PointerMoved(object sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }

    private void MessageBusListen()
    {
        // 菜单切换
        MessageBus.Current.Subscribe(MessageBusType.NavMainBox, args =>
        {
            if (args is not string url)
            {
                return;
            }
            ViewChange(url);
        });

        // 窗口状态切换
        MessageBus.Current.Subscribe(MessageBusType.WindowState, args =>
        {
            if (args is not WindowStateCommandType cmdType)
            {
                return;
            }

            switch (cmdType)
            {
                case WindowStateCommandType.Normal:
                    WindowState = WindowState.Normal;
                    break;
                case WindowStateCommandType.Minimized:
                    WindowState = WindowState.Minimized;
                    break;
                case WindowStateCommandType.Maximized:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowStateCommandType.FullScreen:
                    WindowState = WindowState.FullScreen;
                    break;
                case WindowStateCommandType.Close:
                    Close();
                    break;
                case WindowStateCommandType.Hide:
                    Hide();
                    break;
                case WindowStateCommandType.Show:
                    Show();
                    break;
            }
        });

        // 系统通知
        MessageBus.Current.Subscribe(MessageBusType.Notification, args =>
        {
            _nofityManager?.Show(args);
        });
    }
}
