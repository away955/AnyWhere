using Away.App.Views;

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
#else
        OnlyProcess.Listen("onlyProcess");
#endif   
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, lifetime =>
        {
            lifetime.Startup += (_, _) => Startup();
            lifetime.Exit += (_, _) => Exit();

            void Startup()
            {
                var pluginRegisters = AwayLocator.GetServices<IPluginRegister>(Constant.PluginRegisterServiceKey);
                foreach (var register in pluginRegisters)
                {
                    register.ApplicationStartup();
                }
            }
            void Exit()
            {
                var pluginRegisters = AwayLocator.GetServices<IPluginRegister>(Constant.PluginRegisterServiceKey);
                foreach (var register in pluginRegisters)
                {
                    register.ApplicationExit();
                }
            }
        });
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

        services.AddLogging(o => o.AddSerilog());
        services.AddSqlSugarClient(Constant.DBConn, Constant.DBKey);

        services.AddHttpClient();
        services.AddClipboard();
        services.AddStorageProvider();

        services.AddSingleton<IAppMapper, AppMapper>();
        services.AddSingleton<IAppSettingService, AppSettingService>();
        services.AddSingleton<IAppThemeService, AppThemeService>();
        services.AddScoped<IAppMenuRepository, AppMenuRepository>();
        services.AddScoped<IPluginStoreService, PluginStoreService>();

        // 版本检测
        services.AddScoped<IVersionService, VersionService>();
        services.AddScoped<IUpgradeService, UpgradeService>();

        // 视图
        services.AddViewModel<LeftMenuViewModel>();
        services.AddViewModel<TopHeaderViewModel>();
        services.AddViewModel<MainWindowViewModel>();
        services.AddViewModel<AppViewModel>();
        services.AddView<NotFoundView>("404");
        services.AddView<PluginStoreView, PluginStoreViewModel>("app-store");

        // 插件
        PluginRegisterManager.Register();
    }

}


public static class AppBuilderServiceExtensions
{
    public static AppBuilder UseAwayLocator(this AppBuilder builder, Action<IServiceCollection> configureServices)
    {
        configureServices.Invoke(AwayLocator.Services);
        return builder;
    }
}