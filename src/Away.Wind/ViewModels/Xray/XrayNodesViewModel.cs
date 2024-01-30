using Away.Service.DB.Entities;
using Away.Service.XrayNode;

namespace Away.Wind.ViewModels;

public class XrayNodesViewModel : BindableBase
{
    private readonly IXrayNodeRepository _xrayNodeRepository;
    private readonly IXrayNodeService _xrayNodeService;

    public XrayNodesViewModel(
        IXrayNodeRepository xrayNodeRepository,
        IXrayNodeService xrayNodeService
        )
    {
        _xrayNodeRepository = xrayNodeRepository;
        _xrayNodeService = xrayNodeService;

        ResetCommand = new DelegateCommand(OnResetCommand);
        UpdateNodeCommand = new DelegateCommand(OnUpdateNodeCommand);

        ResetCommand.Execute();
    }

    private ObservableCollection<XrayNodeEntity> _xrayNodeItemsSource;
    public ObservableCollection<XrayNodeEntity> XrayNodeItemsSource
    {
        get => _xrayNodeItemsSource;
        set => SetProperty(ref _xrayNodeItemsSource, value);
    }

    public DelegateCommand ResetCommand { get; private set; }
    private void OnResetCommand()
    {
        var xraynodes = _xrayNodeRepository.GetList();
        _xrayNodeItemsSource = new ObservableCollection<XrayNodeEntity>(xraynodes);
    }

    public DelegateCommand UpdateNodeCommand { get; private set; }
    public void OnUpdateNodeCommand()
    {
        _xrayNodeService.SetXrayNodeByUrl("https://bulinkbulink.com/freefq/free/master/v2");
    }
}
