using Away.Service.Windows;
using Away.Service.Xray.Impl;

namespace Away.Wind;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        if (ProcessOnly.Check("AwayApp20240206"))
        {
            Current.Shutdown();
            Environment.Exit(-1);
            return;
        }
        base.OnStartup(e);
        var bootstrapper = new Bootstrapper();
        bootstrapper.Run();

    }
    protected override void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        XrayService.XraysClose();
    }
}
