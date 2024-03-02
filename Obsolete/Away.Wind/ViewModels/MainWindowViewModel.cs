using Away.Service.Windows;
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
        IMessageService messageService,
        ISettingsRepository settingsRepository,
        IXrayService xrayService,
        IDialogService dialogService,
        IRegionManager regionManager,
        IMenuRepository menuRepository
        )
    {

        messageService.Subscribe(MessageQueue.Enqueue);
        TaskBarIconVM = new(settingsRepository, xrayService);
        TopHeaderVM = new(dialogService);
        LeftMenuVM = new(regionManager, settingsRepository, menuRepository)
        {
            DefaultUrl = "xray-nodes"
        };

        TopHeaderVM.OnToggleButtonChange += (o) => LeftMenuVM.Toggle = o;
        DebugCommand = new(OnDebugCommand);
    }

    public DelegateCommand DebugCommand { get; private set; }
    private void OnDebugCommand()
    {
        ConsoleManager.Toggle();
    }
}