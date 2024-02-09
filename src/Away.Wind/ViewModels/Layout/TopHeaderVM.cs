using System.ComponentModel;
using System.Windows.Input;

namespace Away.Wind.ViewModels;

public class TopHeaderVM : BindableBase
{
    private readonly IDialogService _dialogService;

    public TopHeaderVM(IDialogService dialogService)
    {
        _dialogService = dialogService;

        CheckedToggleButtonCommand = new(OnCheckedToggleButtonCommand);
        UnCheckedToggleButtonCommand = new(OnUnCheckedToggleButtonCommand);

        MenuItemsSource = [
            new TopMenuModel
            {
                Icon = "WindowMaximize",
                ToolTip = "最大化",
                Command = new DelegateCommand<TopMenuModel?>(OnWindowState),
            },
            new TopMenuModel
            {
                Icon = "Close",
                ToolTip = "关闭",
                Command = new DelegateCommand(() => IsHide = true),
            },
        ];
    }


    public event Action<bool>? OnToggleButtonChange;

    /// <summary>
    /// 展开
    /// </summary>
    public DelegateCommand CheckedToggleButtonCommand { get; private set; }
    private void OnCheckedToggleButtonCommand()
    {
        OnToggleButtonChange?.Invoke(true);
    }

    /// <summary>
    /// 收起
    /// </summary>
    public DelegateCommand UnCheckedToggleButtonCommand { get; private set; }
    private void OnUnCheckedToggleButtonCommand()
    {
        OnToggleButtonChange?.Invoke(false);
    }

    private ObservableCollection<TopMenuModel> _menuItemsSource = [];
    /// <summary>
    /// 菜单资源
    /// </summary>
    public ObservableCollection<TopMenuModel> MenuItemsSource
    {
        get => _menuItemsSource;
        set => SetProperty(ref _menuItemsSource, value);
    }

    private bool _isClose;
    public bool IsHide
    {
        get => _isClose;
        set => SetProperty(ref _isClose, value);
    }

    private WindowState _windowState;
    /// <summary>
    /// 窗口状态
    /// </summary>
    public WindowState WindowState
    {
        get => _windowState;
        set => SetProperty(ref _windowState, value);
    }
    private void OnWindowState(TopMenuModel? model)
    {
        if (model == null)
        {
            return;
        }

        if (model.Icon == "WindowMaximize")
        {
            model.Icon = "WindowRestore";
            model.ToolTip = "还原";

            WindowState = WindowState.Maximized;
        }
        else
        {
            model.Icon = "WindowMaximize";
            model.ToolTip = "最大化";
            WindowState = WindowState.Normal;
        }
    }
}

public class TopMenuModel : BindableBase
{
    private string _toolTip = string.Empty;
    private string _icon = string.Empty;

    public string ToolTip
    {
        get => _toolTip;
        set => SetProperty(ref _toolTip, value);
    }
    public string Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }
    public required ICommand Command { get; set; }
}
