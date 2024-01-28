namespace Away.Service.IOC;

[AttributeUsage(AttributeTargets.Class)]
public class NavigationAttribute : Attribute
{
    public NavigationAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}