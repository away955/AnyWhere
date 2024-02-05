namespace Away.Service.IOC;

/// <summary>
/// 标记注册DI
/// </summary>
/// <param name="serviceLifetime"></param>
/// <param name="injectSelf"></param>
[AttributeUsage(AttributeTargets.Class)]
public class ServiceInjectAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, bool injectSelf = false) : Attribute
{
    public ServiceLifetime ServiceLifetime { get; private set; } = serviceLifetime;

    /// <summary>
    /// 注册自己，不依赖接口
    /// </summary>
    public bool InjectSelf { get; private set; } = injectSelf;
}