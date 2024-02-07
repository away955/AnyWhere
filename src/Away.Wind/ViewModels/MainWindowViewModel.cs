namespace Away.Wind.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private TaskBarIconVM _taskBarIconVM = null!;
    public TaskBarIconVM TaskBarIconVM { get => _taskBarIconVM; set => SetProperty(ref _taskBarIconVM, value); }

    private TopHeaderVM _topHeaderVM = null!;
    public TopHeaderVM TopHeaderVM { get => _topHeaderVM; set => SetProperty(ref _topHeaderVM, value); }

    private LeftMenuVM _leftMenuVM = null!;
    public LeftMenuVM LeftMenuVM { get => _leftMenuVM; set => SetProperty(ref _leftMenuVM, value); }

    public MainWindowViewModel(
        TaskBarIconVM taskBarIconVM,
        TopHeaderVM topHeaderVM,
        LeftMenuVM leftMenuVM
        )
    {

        TaskBarIconVM = taskBarIconVM;
        TopHeaderVM = topHeaderVM;
        LeftMenuVM = leftMenuVM;


        TopHeaderVM.OnToggleButtonChange += (o) => LeftMenuVM.Toggle = o;
    }


}