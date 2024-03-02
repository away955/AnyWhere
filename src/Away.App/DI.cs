using Away.App.Core.Extensions.DependencyInjection;
using Mapster;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Away.App;

public sealed class DI
{
    private static void ConfigureLogging()
    {
        var logConf = new LoggerConfiguration();
        logConf.MinimumLevel.Information();
        logConf.WriteTo.Console();
        Log.Logger = logConf.CreateLogger();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
#if DEBUG
        services.AddConsoleManager(true);
#else
        services.AddConsoleManager();
#endif
        ConfigureLogging();
        Log.Information("注册服务");

        services.AddLogging(o => o.AddSerilog());
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
        services.AddMapster();
        services.AddFileContext();
        services.AddClipboard();
        services.AddProxySettings();
    }
}
