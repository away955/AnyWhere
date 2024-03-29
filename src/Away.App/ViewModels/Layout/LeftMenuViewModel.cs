namespace Away.App.ViewModels;

[ViewModel]
public sealed class LeftMenuViewModel : ReactiveObject
{
    public static string Title => AppInfo.Title;
    public static string Version => AppInfo.Version;
    public ICommand NavCommand { get; }

    [Reactive]
    public string DefaultMenu { get; set; } = "xray-node";

    public LeftMenuViewModel()
    {
        NavCommand = ReactiveCommand.Create<string>(OnNavCommand);
    }

    private void OnNavCommand(string url)
    {
        MessageRouter.Go(url);
    }
}
