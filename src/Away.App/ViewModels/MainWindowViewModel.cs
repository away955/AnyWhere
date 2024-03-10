namespace Away.App.ViewModels;

[ViewModel]
public class MainWindowViewModel : ReactiveObject
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";
}
