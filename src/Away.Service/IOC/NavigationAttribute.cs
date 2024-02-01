namespace Away.Service.IOC;

/// <summary>
/// 标记导航
/// </summary>
/// <param name="name"></param>
[AttributeUsage(AttributeTargets.Class)]
public class NavigationAttribute(string? name = null) : Attribute
{
    public string? Name { get; set; } = name;
}