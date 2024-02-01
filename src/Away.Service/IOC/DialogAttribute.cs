namespace Away.Service.IOC;

/// <summary>
/// 标记弹窗内容
/// </summary>
/// <param name="name"></param>
[AttributeUsage(AttributeTargets.Class)]
public class DialogAttribute(string? name = null) : Attribute
{
    public string? Name { get; set; } = name;
}
