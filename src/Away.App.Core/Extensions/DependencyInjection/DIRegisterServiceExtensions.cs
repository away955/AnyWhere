using Away.App.Core.Navigation;
using System.Reflection;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class DIRegisterServiceExtensions
{
    public static void AddAutoDI(this IServiceCollection services, params Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(o => o.DefinedTypes).Where(o => o.GetCustomAttribute<DIAttribute>() != null);
        foreach (var implType in types)
        {
            var isAdd = services.AddView(implType);
            if (isAdd)
            {
                continue;
            }
            isAdd = services.AddViewModel(implType);
            if (isAdd)
            {
                continue;
            }

            services.AddDI(implType);
        }
    }

    private static bool AddView(this IServiceCollection services, TypeInfo implType)
    {
        var attr = implType.GetCustomAttribute<ViewAttribute>();
        if (attr == null)
        {
            return false;
        }
        var serviceType = typeof(IView);
        _ = attr.ServiceLifetime switch
        {
            ServiceLifetime.Singleton => services.AddKeyedSingleton(serviceType, attr.Key, implType),
            ServiceLifetime.Scoped => services.AddKeyedScoped(serviceType, attr.Key, implType),
            ServiceLifetime.Transient => services.AddKeyedTransient(serviceType, attr.Key, implType),
            _ => throw new NotImplementedException()
        };
        return true;
    }

    private static bool AddViewModel(this IServiceCollection services, TypeInfo implType)
    {
        var attr = implType.GetCustomAttribute<ViewModelAttribute>();
        if (attr == null)
        {
            return false;
        }
        _ = attr.ServiceLifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(implType),
            ServiceLifetime.Scoped => services.AddScoped(implType),
            ServiceLifetime.Transient => services.AddTransient(implType),
            _ => throw new NotImplementedException()
        };
        return true;
    }

    private static void AddDI(this IServiceCollection services, TypeInfo implType)
    {
        var attr = implType.GetCustomAttribute<DIAttribute>();
        if (attr == null)
        {
            return;
        }

        // 未实现接口，注册自身
        if (attr.InjectSelf || !implType.ImplementedInterfaces.Any())
        {
            _ = attr.ServiceLifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(implType),
                ServiceLifetime.Scoped => services.AddScoped(implType),
                ServiceLifetime.Transient => services.AddTransient(implType),
                _ => throw new NotImplementedException()
            };
            return;
        }

        var serviceType =
            implType.ImplementedInterfaces.FirstOrDefault(o => o.Name == $"I{implType.Name}")
            ?? implType.ImplementedInterfaces.FirstOrDefault()!;

        if (!string.IsNullOrWhiteSpace(attr.Key))
        {
            _ = attr.ServiceLifetime switch
            {
                ServiceLifetime.Singleton => services.AddKeyedSingleton(serviceType, attr.Key, implType),
                ServiceLifetime.Scoped => services.AddKeyedScoped(serviceType, attr.Key, implType),
                ServiceLifetime.Transient => services.AddKeyedTransient(serviceType, attr.Key, implType),
                _ => throw new NotImplementedException()
            };
            return;
        }

        _ = attr.ServiceLifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implType),
            ServiceLifetime.Transient => services.AddTransient(serviceType, implType),
            _ => throw new NotImplementedException()
        };
    }
}
