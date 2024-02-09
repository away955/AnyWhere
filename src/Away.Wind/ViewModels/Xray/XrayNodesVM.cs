using Away.Service.XrayNode;
using System.Text.RegularExpressions;

namespace Away.Wind.ViewModels;

public class XrayNodesVM : BindableBase
{
    private readonly ILogger<XrayNodesVM> _logger;
    private readonly IXrayNodeRepository _xrayNodeRepository;
    private readonly IXrayNodeService _xrayNodeService;
    private readonly IXrayNodeSubRepository _xrayNodeSubRepository;
    private readonly IXrayService _xrayService;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly IDialogService _dialogService;

    public XrayNodesVM(
        ILogger<XrayNodesVM> logger,
        IXrayNodeRepository xrayNodeRepository,
        IXrayNodeService xrayNodeService,
        IXrayNodeSubRepository xrayNodeSubRepository,
        IXrayService xrayService,
        IMapper mapper,
        IMessageService messageService,
        IDialogService dialogService
        )
    {
        _mapper = mapper;
        _logger = logger;
        _xrayNodeRepository = xrayNodeRepository;
        _xrayNodeService = xrayNodeService;
        _xrayNodeSubRepository = xrayNodeSubRepository;
        _xrayService = xrayService;
        _messageService = messageService;
        _dialogService = dialogService;

        SettingsCommand = new(() => _dialogService.Show("xray-settings"));
        ResetCommand = new(OnResetCommand);
        UpdateNodeCommand = new(OnUpdateNodeCommand);
        CheckedCommand = new(OnCheckedCommand);
        CopyCommand = new(OnCopyCommand);
        PasteCommand = new(OnPasteCommand);
        SpeedTest = new(OnSpeedTest);

        OnResetCommand();
    }

    public DelegateCommand SettingsCommand { get; private set; }

    public DelegateCommand ResetCommand { get; private set; }
    private void OnResetCommand()
    {
        IsEnableXray = _xrayService.IsEnable;
        IsEnableGlobalProxy = _xrayService.IsEnableGlobalProxy;
        var xraynodes = _xrayNodeRepository.GetList();
        var items = xraynodes.Select(_mapper.Map<XrayNodeModel>);
        XrayNodeItemsSource = new ObservableCollection<XrayNodeModel>(items);
    }

    private bool _isEnableXray;
    /// <summary>
    /// 是否启用xray
    /// </summary>
    public bool IsEnableXray
    {
        get => _isEnableXray;
        set
        {
            SetProperty(ref _isEnableXray, value);
            if (_isEnableXray)
            {
                _xrayService.XrayStart();
                var inbound = _xrayService.Config.inbounds.FirstOrDefault();
                if (inbound == null)
                {
                    return;
                }
            }
            else
            {
                _xrayService.XrayClose();
            }
        }
    }

    private bool _isEnableGlobalProxy;
    /// <summary>
    /// 是否启用全局网络代理
    /// </summary>
    public bool IsEnableGlobalProxy
    {
        get => _isEnableGlobalProxy;
        set
        {
            SetProperty(ref _isEnableGlobalProxy, value);
            if (_isEnableGlobalProxy)
            {
                _xrayService.OpenGlobalProxy();
            }
            else
            {
                _xrayService.CloseGlobalProxy();
            }
        }
    }

    private ObservableCollection<XrayNodeModel> _xrayNodeItemsSource = [];
    /// <summary>
    /// 节点列表
    /// </summary>
    public ObservableCollection<XrayNodeModel> XrayNodeItemsSource
    {
        get => _xrayNodeItemsSource;
        set => SetProperty(ref _xrayNodeItemsSource, value);
    }

    /// <summary>
    /// 更新节点
    /// </summary>
    public DelegateCommand UpdateNodeCommand { get; private set; }
    public async void OnUpdateNodeCommand()
    {
        var subs = _xrayNodeSubRepository.AsQueryable().Where(o => o.IsDisable == false).ToList();
        foreach (var sub in subs)
        {
            await _xrayNodeService.SetXrayNodeByUrl(sub.Url);
        }
        OnResetCommand();
        _messageService.Show("节点订阅更新完成");
    }

    /// <summary>
    /// 选择代理节点
    /// </summary>
    public DelegateCommand<XrayNodeModel?> CheckedCommand { get; private set; }
    private void OnCheckedCommand(XrayNodeModel? model)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Url))
        {
            return;
        }

        var entity = _mapper.Map<XrayNodeEntity>(model);
        _xrayService.Config.SetOutbound(entity);
        _xrayService.SaveConfig();
        _xrayService.XrayRestart();
        IsEnableXray = _xrayService.IsEnable;

        // 设置选中节点
        foreach (var item in XrayNodeItemsSource)
        {
            item.IsChecked = item.Id == model.Id;
        }
        var items = XrayNodeItemsSource.Select(_mapper.Map<XrayNodeEntity>).ToList();
        _xrayNodeRepository.SaveNodes(items);
        _messageService.Show("切换节点成功");
    }

    /// <summary>
    /// 测试节点速度
    /// </summary>
    public DelegateCommand SpeedTest { get; private set; }
    private void OnSpeedTest()
    {
        _messageService.Show("开始测试");
        var items = XrayNodeItemsSource.Select(_mapper.Map<XrayNodeEntity>).ToList();
        var speedService = new XrayNodeSpeedTest(items, 10, 3000);
        speedService.OnTesting += (entity) =>
        {
            var model = XrayNodeItemsSource.FirstOrDefault(o => o.Id == entity.Id);
            if (model == null)
            {
                return;
            }
            model.Status = XrayNodeStatus.Default;
            model.Remark = "检测中...";
        };

        speedService.OnCompeleted += async (e) =>
        {
            var result = e.Data;
            var entity = e.XrayNode;
            var model = XrayNodeItemsSource.FirstOrDefault(o => o.Id == entity.Id)!;

            if (result.IsSuccess)
            {
                model.Status = entity.Status = XrayNodeStatus.Success;
                model.Remark = entity.Remark = result.Speed;
            }
            else
            {
                model.Status = entity.Status = XrayNodeStatus.Error;
                model.Remark = entity.Remark = result.Error;
            }
            await _xrayNodeRepository.UpdateAsync(entity);

        };
        speedService.Listen(() => _messageService.Show("节点测试完成"));
    }

    /// <summary>
    /// 复制节点
    /// </summary>
    public DelegateCommand<XrayNodeModel?> CopyCommand { get; private set; }
    public static void OnCopyCommand(XrayNodeModel? entity)
    {
        if (entity == null)
        {
            return;
        }
        Clipboard.SetText(entity.Url);
    }

    /// <summary>
    /// 粘贴节点
    /// </summary>
    public DelegateCommand PasteCommand { get; private set; }
    public async void OnPasteCommand()
    {
        var text = Clipboard.GetText();

        var items = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in items)
        {
            var base64Pattern = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";
            if (Regex.IsMatch(text, base64Pattern))
            {
                await _xrayNodeService.SetXrayNodeByBase64String(text);
            }
            else
            {
                await _xrayNodeService.SaveXrayNodeByList([text]);
            }
        }
        OnResetCommand();
    }
}