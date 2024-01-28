using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceInjectExtension
{
    public static void AddAwayDI(this IServiceCollection services)
    {
        var types = typeof(ServiceInjectExtension).Assembly.DefinedTypes
            .Where(t => t.GetCustomAttribute<ServiceInjectAttribute>() != null);
        foreach (var implType in types)
        {
            services.AddDefaultInject(implType);
        }
    }



    private static void AddDefaultInject(this IServiceCollection services, TypeInfo implType)
    {
        var attr = implType.GetCustomAttribute<ServiceInjectAttribute>();
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

        // 标准接口与实现类对应，注入接口
        var serviceType =
            implType.ImplementedInterfaces.FirstOrDefault(o => o.Name == $"I{implType.Name}")
            ?? implType.ImplementedInterfaces.FirstOrDefault()!;
        _ = attr.ServiceLifetime switch
        {
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implType),
            ServiceLifetime.Transient => services.AddTransient(serviceType, implType),
            _ => throw new NotImplementedException()
        };
    }
}