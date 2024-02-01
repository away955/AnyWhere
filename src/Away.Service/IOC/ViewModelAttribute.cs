namespace Away.Service.IOC;

/// <summary>
/// 标记 ViewModel
/// </summary>
/// <param name="viewModelType"></param>
[AttributeUsage(AttributeTargets.Class)]
public class ViewModelAttribute(Type viewModelType) : Attribute
{
    public Type ViewModelType { get; private set; } = viewModelType;
}
