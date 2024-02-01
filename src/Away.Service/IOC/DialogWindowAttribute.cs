namespace Away.Service.IOC;

/// <summary>
/// 标记弹窗主题
/// </summary>
/// <param name="name"></param>
[AttributeUsage(AttributeTargets.Class)]
public class DialogWindowAttribute(string? name = null) : Attribute
{
    public string? Name { get; set; } = name;
}
