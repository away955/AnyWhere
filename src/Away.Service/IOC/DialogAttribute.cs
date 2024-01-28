namespace Away.Service.IOC;

[AttributeUsage(AttributeTargets.Class)]
public class DialogAttribute : Attribute
{
    public DialogAttribute(string? name = null)
    {
        Name = name;
    }
    public string? Name { get; set; }
}
