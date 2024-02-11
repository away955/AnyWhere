using MaterialDesignThemes.Wpf;

namespace Away.Wind.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private TaskBarIconVM _taskBarIconVM = null!;
    public TaskBarIconVM TaskBarIconVM { get => _taskBarIconVM; set => SetProperty(ref _taskBarIconVM, value); }

    private TopHeaderVM _topHeaderVM = null!;
    public TopHeaderVM TopHeaderVM { get => _topHeaderVM; set => SetProperty(ref _topHeaderVM, value); }

    private LeftMenuVM _leftMenuVM = null!;
    public LeftMenuVM LeftMenuVM { get => _leftMenuVM; set => SetProperty(ref _leftMenuVM, value); }

    private SnackbarMessageQueue _messageQueue = new();
    public SnackbarMessageQueue MessageQueue
    {
        get => _messageQueue;
        set => SetProperty(ref _messageQueue, value);
    }

    public MainWindowViewModel(
        TaskBarIconVM taskBarIconVM,
        TopHeaderVM topHeaderVM,
        LeftMenuVM leftMenuVM,
        IMessageService messageService
        )
    {

        TaskBarIconVM = taskBarIconVM;
        TopHeaderVM = topHeaderVM;
        LeftMenuVM = leftMenuVM;

        messageService.Subscribe(ShowMessage);

        TopHeaderVM.OnToggleButtonChange += (o) => LeftMenuVM.Toggle = o;
    }

    private void ShowMessage(string text)
    {
        MessageQueue.Enqueue(text);
    }


}