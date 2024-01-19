using Away.Wind.ViewModels;
using Away.Wind.Views;

namespace Away.Wind;

public class Bootstrapper : PrismBootstrapper
{
    protected override DependencyObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
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