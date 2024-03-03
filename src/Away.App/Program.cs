using Avalonia.Controls.ApplicationLifetimes;

namespace Away.App;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, lifetime =>
        {
            lifetime.Exit += Lifetime_Exit;
        });
    }

    private static void Lifetime_Exit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {      
        var _xrayService = AwayLocator.ServiceProvider.GetService<IXrayService>();
        _xrayService?.CloseAll();
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UseAwayLocator(DI.ConfigureServices)
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }
}
