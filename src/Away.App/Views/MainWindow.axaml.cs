using Avalonia.Controls.Primitives;

namespace Away.App.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    private WindowNotificationManager? _nofityManager;

    public MainWindow()
    {
        ViewModel = AwayLocator.GetService<MainWindowViewModel>();
        this.InitializeComponent();
        MessageBusListen();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _nofityManager = new WindowNotificationManager(this)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 3,
        };
        base.OnApplyTemplate(e);
    }

    public void TopHeader_PointerMoved(object sender, PointerPressedEventArgs e)
    {
        this.BeginMoveDrag(e);
    }

    private void MessageBusListen()
    {
        //系统消息
        MessageShow.Listen(args => _nofityManager?.Show(args));
        // 菜单切换
        MessageRouter.Listen(args =>
        {
            if (args is not string url)
            {
                return;
            }
            Log.Information($"router:{url}");
            var view = AwayLocator.ServiceProvider.GetView(url) ?? AwayLocator.ServiceProvider.GetView("404");
            this.MainBox.Content = view;
        });
        // 窗口状态切换
        MessageWindowState.Listen(args =>
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
                case WindowStateCommandType.Activate:
                    Activate();
                    break;
                case WindowStateCommandType.ShowActivate:
                    Show();
                    Activate();
                    break;
            }
        });

    }
}
