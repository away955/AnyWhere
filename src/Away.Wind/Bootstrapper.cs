using Away.Wind.Components;
using Away.Wind.ViewModels;
using Away.Wind.Views;
using System.Reflection;
using System.Windows.Controls;

namespace Away.Wind;

public class Bootstrapper : PrismBootstrapper
{
    protected override DependencyObject CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
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

        //ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
        //{
        //    var viewName = viewType.FullName;
        //    var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
        //    var viewModelName = $"{viewName}ViewModel, {viewAssemblyName}";
        //    return Type.GetType(viewModelName);
        //});
        ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
    }

    protected override IModuleCatalog CreateModuleCatalog()
    {
        return new ConfigurationModuleCatalog();
    }
}