namespace Away.App.ViewModels;

[ViewModel]
public class MainWindowViewModel : ViewModelBase
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";
}
