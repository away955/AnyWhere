using Away.Wind.Views;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Prism.DryIoc;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Reflection;

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

    protected override Rules CreateContainerRules()
    {
        return Rules.Default.WithConcreteTypeDynamicRegistrations(reuse: Reuse.Transient)
                            .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
                            .WithFuncAndLazyWithoutRegistration()
                            .WithTrackingDisposableTransients()
                            .WithFactorySelector(Rules.SelectLastRegisteredFactory());
    }

    protected override IContainerExtension CreateContainerExtension()
    {
        var container = new Container(CreateContainerRules());
        var services = new ServiceCollection();
        AddServiceCollections(services);
        container.WithDependencyInjectionAdapter(services);
        return new DryIocContainerExtension(container);
    }

    private void AddServiceCollections(IServiceCollection services)
    {
        services.AddSingleton(Configuration);
        services.AddLogging(o => o.AddSerilog());
        services.AddHttpClient("xray")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (m, c, ch, e) => true
            });
        var connStr = Configuration.GetConnectionString("Sqlite");
        services.AddSqlSugarClient(connStr!);
        services.AddAwayDI();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        var types = typeof(Bootstrapper).Assembly.DefinedTypes;
        foreach (var type in types)
        {
            var nav = type.GetCustomAttribute<NavigationAttribute>();
            if (nav != null)
            {
                containerRegistry.Register(typeof(object), type, nav.Name);
                continue;
            }

            var dialog = type.GetCustomAttribute<DialogAttribute>();
            if (dialog != null)
            {
                containerRegistry.Register(typeof(object), type, dialog.Name);
                continue;
            }

            var dialogWind = type.GetCustomAttribute<DialogWindowAttribute>();
            if (dialogWind != null)
            {
                containerRegistry.Register(typeof(Prism.Services.Dialogs.IDialogWindow), type, dialogWind.Name);
                continue;
            }
        }

    }    

    protected override void ConfigureViewModelLocator()
    {
        base.ConfigureViewModelLocator();
        ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
        {
            // 自定义：属性标记
            var vmType = viewType.GetCustomAttribute<ViewModelAttribute>()?.ViewModelType;
            if (vmType != null)
            {
                return vmType;
            }

            // 默认方式
            string fullName = viewType.FullName!;
            fullName = fullName.Replace(".Views.", ".ViewModels.");
            string fullName2 = viewType.GetTypeInfo().Assembly.FullName!;
            string arg = (fullName.EndsWith("View") ? "Model" : "ViewModel");
            return Type.GetType(string.Format(CultureInfo.InvariantCulture, "{0}{1}, {2}", fullName, arg, fullName2));
        });
    }

    //protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    //{

    //}
}