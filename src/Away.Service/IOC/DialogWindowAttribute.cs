namespace Away.Service.IOC;

[AttributeUsage(AttributeTargets.Class)]
public class DialogWindowAttribute : Attribute
{
    public DialogWindowAttribute(string? name = null)
    {
        Name = name;
    }
    public string? Name { get; set; }
}
