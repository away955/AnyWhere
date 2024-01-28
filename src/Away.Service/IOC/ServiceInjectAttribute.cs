using Microsoft.Extensions.DependencyInjection;

namespace Away.Service.IOC;

[AttributeUsage(AttributeTargets.Class)]
public class ServiceInjectAttribute : Attribute
{
    public ServiceInjectAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, bool injectSelf = false)
    {
        ServiceLifetime = serviceLifetime;
        InjectSelf = injectSelf;
    }

    public ServiceLifetime ServiceLifetime { get; private set; }

    /// <summary>
    /// 注册自己，不依赖接口
    /// </summary>
    public bool InjectSelf { get; private set; }
}