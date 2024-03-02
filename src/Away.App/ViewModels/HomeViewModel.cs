namespace Away.App.ViewModels;

[ViewModel]
public sealed class HomeViewModel : ReactiveObject
{
    public ICommand TestCommand { get; }
    public ICommand TestCommand3 { get; }
    public HomeViewModel()
    {
        TestCommand = ReactiveCommand.Create(() => MessageBus.Current.Nofity("标题", "内容111"));
        TestCommand3 = ReactiveCommand.Create(() => Log.Information("333"));
    }

}
