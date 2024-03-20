namespace Away.App.Core.DI;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute(string key, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) : DIAttribute(key, serviceLifetime)
{
}
