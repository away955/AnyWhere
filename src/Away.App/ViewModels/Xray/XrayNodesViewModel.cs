using Avalonia.Input.Platform;
using System.Threading.Tasks;

namespace Away.App.ViewModels;

[ViewModel]
internal class XrayNodesViewModel : ViewModelBase
{
    private readonly IXrayNodeRepository _xrayNodeRepository;
    private readonly IXrayNodeService _xrayNodeService;
    private readonly IXrayNodeSubRepository _xrayNodeSubRepository;
    private readonly IXrayService _xrayService;
    private readonly IMapper _mapper;
    private readonly IClipboard _clipboard;

    public ICommand ResetCommand { get; }
    public ICommand UpdateNodeCommand { get; }
    public ICommand CheckedCommand { get; }
    public ICommand SpeedTest { get; }
    public ICommand CopyCommand { get; }
    public ICommand PasteCommand { get; }
    public ICommand DeleteCommand { get; }

    [Reactive]
    public bool IsProgress { get; set; }
    [Reactive]
    public int ProgressValue { get; set; }
    [Reactive]
    public string ProgressText { get; set; } = string.Empty;
    public ICommand ProgressCancelCommand { get; }
    private Action? _progressCancel = null!;

    public XrayNodesViewModel(
        IXrayNodeRepository xrayNodeRepository,
        IXrayNodeService xrayNodeService,
        IXrayNodeSubRepository xrayNodeSubRepository,
        IXrayService xrayService,
        IMapper mapper,
        IClipboard clipboard
        )
    {
        _mapper = mapper;
        _xrayNodeRepository = xrayNodeRepository;
        _xrayNodeService = xrayNodeService;
        _xrayNodeSubRepository = xrayNodeSubRepository;
        _xrayService = xrayService;
        _clipboard = clipboard;

        ResetCommand = ReactiveCommand.Create(OnResetCommand);
        UpdateNodeCommand = ReactiveCommand.Create(OnUpdateNodeCommand);
        CheckedCommand = ReactiveCommand.Create(OnCheckedCommand);
        SpeedTest = ReactiveCommand.Create(OnSpeedTest);
        CopyCommand = ReactiveCommand.Create(OnCopyCommand);
        PasteCommand = ReactiveCommand.Create(OnPasteCommand);
        DeleteCommand = ReactiveCommand.Create(OnDeleteCommand);
        ProgressCancelCommand = ReactiveCommand.Create(OnProgressCancelCommand);
        OnResetCommand();
        MessageBus.Current.Subscribe(MessageBusType.Event, o => OnCheckedCommand(), "DGXrayNode");
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
            this.RaiseAndSetIfChanged(ref _isEnableXray, value);
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
            this.RaiseAndSetIfChanged(ref _isEnableGlobalProxy, value);
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

    /// <summary>
    /// 节点列表
    /// </summary>
    [Reactive]
    public ObservableCollection<XrayNodeModel> XrayNodeItemsSource { get; set; } = [];

    [Reactive]
    public XrayNodeModel? XrayNodeSelectedItem { get; set; }

    /// <summary>
    /// 刷新
    /// </summary>
    private void OnResetCommand()
    {
        IsEnableXray = _xrayService.IsEnable;
        IsEnableGlobalProxy = _xrayService.IsEnableGlobalProxy;
        var xraynodes = _xrayNodeRepository.GetList();
        var items = xraynodes.Select(_mapper.Map<XrayNodeModel>);
        XrayNodeItemsSource = new ObservableCollection<XrayNodeModel>(items);
    }

    /// <summary>
    /// 使用节点
    /// </summary>
    private void OnCheckedCommand()
    {
        var model = XrayNodeSelectedItem;
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
            item.IsChecked = item.Url == model.Url;
        }
        var items = XrayNodeItemsSource.Select(_mapper.Map<XrayNodeEntity>).ToList();
        _xrayNodeRepository.SaveNodes(items);
        Show("切换节点成功");
        OnResetCommand();
    }

    /// <summary>
    /// 更新订阅
    /// </summary>
    private async void OnUpdateNodeCommand()
    {
        if (IsProgress)
        {
            Show($"等待{ProgressText}完成", NotificationType.Warning);
            return;
        }
        var subs = _xrayNodeSubRepository.GetListByEnable();
        var total = subs.Count * 1d;
        if (total > 0)
        {
            ProgressText = "更新订阅";
            IsProgress = true;
        }
        _progressCancel = () =>
        {
            IsProgress = false;
            ProgressValue = 0;
        };


        for (var i = 0; i < total; i++)
        {
            if (!IsProgress)
            {
                break;
            }
            var sub = subs[i];
            Show($"正在更新订阅{sub.Remark}...");
            var url = sub.ParseUrl();
            await _xrayNodeService.SetXrayNodeByUrl(url);
            ProgressValue = (int)((i + 1) / total * 100);
            OnResetCommand();
        }
        OnResetCommand();
        Show("节点订阅更新完成");
        await Task.Delay(1000);
        IsProgress = false;
        ProgressValue = 0;
    }

    /// <summary>
    /// 测试所有节点速度
    /// </summary>
    private void OnSpeedTest()
    {
        if (IsProgress)
        {
            Show($"等待{ProgressText}完成", NotificationType.Warning);
            return;
        }

        Show("开始测试");
        var items = XrayNodeItemsSource.Select(_mapper.Map<XrayNodeEntity>).ToList();
        var total = items.Count * 1d;
        var currentTotal = 0;
        if (total > 0)
        {
            ProgressText = "检测节点";
            IsProgress = true;
        }
        var speedService = new XrayNodeSpeedTest(items, 10, 3000);
        _progressCancel = speedService.Cancel;

        speedService.OnCancel += () =>
        {
            IsProgress = false;
            ProgressValue = 0;
        };
        speedService.OnTesting += (entity) =>
        {
            var model = XrayNodeItemsSource.FirstOrDefault(o => o.Url == entity.Url);
            if (model == null)
            {
                return;
            }
            model.Status = XrayNodeStatus.Default;
            model.Remark = "检测中...";
        };

        speedService.OnTested += (e) =>
        {
            var result = e.Data;
            var entity = e.XrayNode;
            var model = XrayNodeItemsSource.FirstOrDefault(o => o.Url == entity.Url)!;

            if (result.IsSuccess)
            {
                model.Status = entity.Status = XrayNodeStatus.Success;
                model.Remark = entity.Remark = result.Speed;
                model.Updated = DateTime.Now;
            }
            else
            {
                model.Status = entity.Status = XrayNodeStatus.Error;
                model.Remark = entity.Remark = result.Error;
            }
            currentTotal++;
            ProgressValue = (int)(currentTotal / total * 100);
        };

        speedService.OnCompeleted += async () =>
        {
            _xrayNodeRepository.SaveNodes(items);
            await _xrayNodeRepository.DeleteByStatusError();
            OnResetCommand();
            await Task.Delay(1000);
            IsProgress = false;
            ProgressValue = 0;
        };
        speedService.Listen();
    }

    /// <summary>
    /// 取消任务
    /// </summary>
    private void OnProgressCancelCommand()
    {
        _progressCancel?.Invoke();
    }

    /// <summary>
    /// 复制节点
    /// </summary>
    private async void OnCopyCommand()
    {
        var model = XrayNodeSelectedItem;
        if (model == null)
        {
            return;
        }

        if (_clipboard != null)
        {
            await _clipboard.SetTextAsync(model.Url);
        }
    }

    /// <summary>
    /// 粘贴节点
    /// </summary>
    private async void OnPasteCommand()
    {
        var text = string.Empty;
        if (_clipboard != null)
        {
            text = await _clipboard.GetTextAsync() ?? string.Empty;
        }
        var items = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in items)
        {
            var base64Pattern = "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$";
            if (System.Text.RegularExpressions.Regex.IsMatch(item, base64Pattern))
            {
                _xrayNodeService.SetXrayNodeByBase64String(item);
            }
            else
            {
                _xrayNodeService.SaveXrayNodeByList([item]);
            }
        }
        OnResetCommand();
    }

    /// <summary>
    /// 删除节点
    /// </summary>
    private void OnDeleteCommand()
    {
        if (XrayNodeSelectedItem == null)
        {
            return;
        }
        _xrayNodeRepository.DeleteByUrl(XrayNodeSelectedItem.Url);
        XrayNodeItemsSource.Remove(XrayNodeSelectedItem);
    }

}
