namespace Away.App.Core.DI;

[AttributeUsage(AttributeTargets.Class)]
public class ViewModelAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) : DIAttribute(true, serviceLifetime)
{
}
