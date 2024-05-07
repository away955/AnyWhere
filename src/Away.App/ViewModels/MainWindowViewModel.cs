namespace Away.App.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    public static string Title => $"{Constant.Title} {Constant.Version}";
}
