namespace Away.App.ViewModels;

public sealed class LeftMenuViewModel : ReactiveObject
{
    public static string Title => Constant.Title;
    public static string Version => Constant.Version;
    public ICommand NavCommand { get; }

    [Reactive]
    public string DefaultMenu { get; set; }

    public LeftMenuViewModel()
    {
        NavCommand = ReactiveCommand.Create<string>(OnNavCommand);
    }

    private void OnNavCommand(string url)
    {
        MessageRouter.Go(url);
    }
}
