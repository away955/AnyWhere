using Away.App.Services;
using Away.App.Services.Impl;

namespace Away.App.Update;

public sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
                .UseAwayLocator(ConfigureServices)
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }


    public static void ConfigureServices(IServiceCollection services)
    {

#if DEBUG
        var logConf = new LoggerConfiguration();
        logConf.MinimumLevel.Information();
        logConf.WriteTo.Console();
        Log.Logger = logConf.CreateLogger();

        Log.Information("注册服务");
        services.AddLogging(o => o.AddSerilog());
#endif

        services.AddHttpClient("xray")
          .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
          {
              ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
          });

        services.AddSingleton<MainWindowViewModel>();
        services.AddScoped<IVersionService, VersionService>();
        services.AddScoped<IUpgradeService, UpgradeService>();
    }
}

public static class AppBuilderServiceExtensions
{
    public static AppBuilder UseAwayLocator(this AppBuilder builder, Action<IServiceCollection> configureServices)
    {
        configureServices?.Invoke(App.Services);
        return builder;
    }
}
