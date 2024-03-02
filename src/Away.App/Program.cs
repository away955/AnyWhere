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
        if (sender is not IClassicDesktopStyleApplicationLifetime lifetime)
        {
            return;
        }
        var serviceProvider = lifetime.GetServiceProvider();
        var _xrayService = serviceProvider.GetService<IXrayService>();
        _xrayService?.CloseAll();
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UserContaierFactory(DI.ConfigureServices)
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }    
}
