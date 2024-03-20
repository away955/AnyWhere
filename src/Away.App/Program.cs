using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Core.IPC;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Away.App;

public sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
#if DEBUG
        var logConf = new LoggerConfiguration();
        logConf.MinimumLevel.Information();
        logConf.WriteTo.Console();
        Log.Logger = logConf.CreateLogger();
#endif
        OnlyProcess.HasLiveAction = (cmd) => MessageBus.Current.Publish(MessageBusType.WindowState, cmd);
        OnlyProcess.Listen("onlyProcess");

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
            .UseAwayLocator(ConfigureServices)
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        Log.Information("注册服务");
#if DEBUG
        services.AddLogging(o => o.AddSerilog());
#endif
        services.AddHttpClient("xray")
          .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
          {
              UseProxy = false,
              ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
          });

        services.Configure<JsonSerializerOptions>(options =>
        {
            options.NumberHandling = JsonNumberHandling.AllowReadingFromString;
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.WriteIndented = true;
            options.PropertyNameCaseInsensitive = true;
        });
        var libs = AppDomain.CurrentDomain.GetAssemblies().Where(o => o.FullName!.StartsWith("Away.App"));
        services.AddAutoDI([.. libs, typeof(XrayConfig).Assembly]);
        services.AddFileContext();
        services.AddClipboard();
        services.AddProxySettings();
        services.AddScoped<IVersionService, VersionService>();
        services.AddScoped<IUpdateService, UpdateService>();
    }

}
