using Avalonia.Controls.ApplicationLifetimes;
using Away.App.Core.IPC;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

        CheckOnlyProcess();
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

#if DEBUG
        Log.Information("注册服务");
        services.AddLogging(o => o.AddSerilog());
#endif

        services.AddHttpClient("xray")
          .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
          {
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

    private static async void CheckOnlyProcess()
    {
        const string pipeName = "onlyProcess";
        try
        {
            using IPCClient ipcClient = new(".", pipeName);
            ipcClient.OnReceive += (cmd) =>
            {
                Environment.Exit(0);
            };
            ipcClient.Connect(TimeSpan.FromSeconds(1));
            await ipcClient.CommandAsync(WindowStateCommandType.ShowActivate);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "IPC Client Connecting Error");
        }
        _ = IPCListen(pipeName);

    }

    private static async Task IPCListen(string pipeName)
    {
        try
        {
            while (true)
            {
                using IPCServer ipcServer = new(pipeName);
                ipcServer.OnReceive += (cmd) =>
                {
                    MessageBus.Current.Publish(MessageBusType.WindowState, cmd);
                };
                await ipcServer.Listen();
            }
        }
        catch (Exception ex)
        {
            Log.Information(ex, "IPC Server Starting Error");
        }
    }

}
