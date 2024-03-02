namespace Away.App.Core.DI;

[AttributeUsage(AttributeTargets.Class)]
public class ViewAttribute(string key, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) : DIAttribute(key, serviceLifetime)
{
}
