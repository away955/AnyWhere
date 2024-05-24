using Away.App.Core.MVVM;

namespace Away.App.Core.Extensions;

public static class MVVMServiceExtensions
{
    public static IServiceCollection AddView<TView, TViewModel>(this IServiceCollection services, string key)
        where TView : class, IView
        where TViewModel : ReactiveObject
    {
        return services.AddView<TView>(key).AddViewModel<TViewModel>();
    }

    public static IServiceCollection AddView<TView>(this IServiceCollection services, string key) where TView : class, IView
    {
        return services.AddKeyedSingleton<IView, TView>(key);
    }

    public static IServiceCollection AddViewModel<TViewModel>(this IServiceCollection services) where TViewModel : ReactiveObject
    {
        return services.AddSingleton<TViewModel>();
    }

    public static IView? GetView(this IServiceProvider provider, string? url)
    {
        return provider.GetKeyedService<IView>(url);
    }
}


