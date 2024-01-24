using Away.Service;
using Away.Wind.ViewModels;
using Away.Wind.Views;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Away.Wind;

public sealed class Bootstrapper : PrismBootstrapper
{
    public static IConfiguration Configuration { get; }

    static Bootstrapper()
    {
        Configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
        .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();
    }
    protected override DependencyObject CreateShell()
    {
        Log.Logger.Information("启动成功");
        return Container.Resolve<MainWindow>();
    }

    private void AddServiceCollections(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        services.AddLogging(o => o.AddSerilog());
        services.AddHttpClient();

        services.AddScoped<IDemo, Demo>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterServices(AddServiceCollections);

        containerRegistry.RegisterForNavigation<Home>("home");
        containerRegistry.RegisterForNavigation<MenuSettings>("menu-settings");
        containerRegistry.RegisterForNavigation<Settings>("settings");
        containerRegistry.RegisterForNavigation<NotFound>("404");
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
        //var moduleAType = typeof(ModuleAModule);
        //moduleCatalog.AddModule(new ModuleInfo()
        //{
        //    ModuleName = moduleAType.Name,
        //    ModuleType = moduleAType.AssemblyQualifiedName,
        //    InitializationMode = InitializationMode.OnDemand
        //});
    }

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();

        ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        //ViewModelLocationProvider.Register<MenuSettings, MenuSettingsViewModel>();
        //ViewModelLocationProvider.Register<Settings, SettingsViewModel>();
    }

    protected override IModuleCatalog CreateModuleCatalog()
    {
        return new ConfigurationModuleCatalog();
    }
}