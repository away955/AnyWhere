namespace Away.App.ViewModels;

[ViewModel]
public sealed class TopHeaderViewModel : ReactiveObject
{
    private static readonly string Maximum = IconData.Current["Maximum"].ToUnicode();
    private static readonly string Normal = IconData.Current["Normal"].ToUnicode();

    public ICommand CloseCommand { get; }
    public ICommand NormalCommand { get; }
    public ICommand MinimizedCommand { get; }



    [Reactive]
    public string NormalIcon { get; set; } = Maximum;

    public TopHeaderViewModel()
    {
        CloseCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Hide));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Minimized));
        NormalCommand = ReactiveCommand.Create(OnNomalCommand);
    }


    private void OnNomalCommand()
    {
        if (NormalIcon == Maximum)
        {
            OnCommand(WindowStateCommandType.Maximized);
            NormalIcon = Normal;
        }
        else
        {
            OnCommand(WindowStateCommandType.Normal);
            NormalIcon = Maximum;
        }
    }

    private static void OnCommand(WindowStateCommandType state)
    {
        MessageBus.Current.Publish(MessageBusType.WindowState, state);
    }

}
