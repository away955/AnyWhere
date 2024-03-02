namespace Away.App.Core.DI;

[AttributeUsage(AttributeTargets.Class)]
public class DIAttribute : Attribute
{
    public DIAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ServiceLifetime = serviceLifetime;
    }

    public DIAttribute(bool injectSelf, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ServiceLifetime = serviceLifetime;
        InjectSelf = injectSelf;
    }

    public DIAttribute(string key, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        ServiceLifetime = serviceLifetime;
        Key = key;
    }

    public ServiceLifetime ServiceLifetime { get; private set; }

    /// <summary>
    /// 注册自己，不依赖接口
    /// </summary>
    public bool InjectSelf { get; private set; }

    public string? Key { get; private set; }
}