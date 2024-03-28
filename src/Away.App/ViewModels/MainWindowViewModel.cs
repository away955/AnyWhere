namespace Away.App.ViewModels;

[ViewModel]
public sealed class MainWindowViewModel : ViewModelBase
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";
}
