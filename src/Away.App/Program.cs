using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Core.IPC;
using System.IO;
using System.Net;
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
        var pathRoot = Environment.CurrentDirectory.Replace("\\bin\\Debug\\net8.0", string.Empty);
        var conn = Path.Combine(pathRoot, "Data", "away.sqlite");
        services.AddSqlSugarClient($"DataSource={conn}");
#else
        services.AddSqlSugarClient("DataSource=./Data/away.sqlite");
#endif
        services.AddHttpClient("unsafe")
          .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
          {
              ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
          });

        services.AddHttpClient("xray-proxy")
         .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
         {
             Proxy = new WebProxy("127.0.0.1", 1080),
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
        services.AddClipboard();
        services.AddStorageProvider();
        services.AddProxySettings();
        services.AddScoped<IVersionService, VersionService>();
        services.AddScoped<IUpdateService, UpdateService>();
    }

}
