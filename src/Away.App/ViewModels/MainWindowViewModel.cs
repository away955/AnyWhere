using Away.App.Core.Windows.Consoles;

namespace Away.App.ViewModels;

[ViewModel]
public class MainWindowViewModel : ReactiveObject
{
    public static string Title => $"{AppInfo.Title} {AppInfo.Version}";

    public ICommand ConsoleCommand { get; }
    public MainWindowViewModel(IConsoleManager consoleManager)
    {
        ConsoleCommand = ReactiveCommand.Create(consoleManager.Toggle);

    }

}
