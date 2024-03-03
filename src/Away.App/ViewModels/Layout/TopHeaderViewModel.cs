namespace Away.App.ViewModels;

[ViewModel]
public sealed class TopHeaderViewModel : ReactiveObject
{
    public ICommand CloseCommand { get; }
    public ICommand NormalCommand { get; }
    public ICommand MinimizedCommand { get; }

    [Reactive]
    public string NormalIcon { get; set; } = Icon.Maximum.ToUnicode();

    public TopHeaderViewModel()
    {
        CloseCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Hide));
        MinimizedCommand = ReactiveCommand.Create(() => OnCommand(WindowStateCommandType.Minimized));
        NormalCommand = ReactiveCommand.Create(OnNomalCommand);

    }


    private void OnNomalCommand()
    {
        if (NormalIcon == Icon.Maximum.ToUnicode())
        {
            OnCommand(WindowStateCommandType.Maximized);
            NormalIcon = Icon.Normal.ToUnicode();
        }
        else
        {
            OnCommand(WindowStateCommandType.Normal);
            NormalIcon = Icon.Maximum.ToUnicode();
        }
    }

    private static void OnCommand(WindowStateCommandType state)
    {
        MessageBus.Current.Publish(MessageBusType.WindowState, state);
    }

}
