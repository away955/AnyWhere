using Avalonia.Input.Platform;
using Away.App.Domain.Xray;
using System.Threading.Tasks;

namespace Away.App.ViewModels;

[ViewModel]
internal class XrayNodesViewModel : ViewModelBase
{
    private readonly IXrayNodeSubService _subService;
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
    private void ClearProgress()
    {
        IsProgress = false;
        ProgressValue = 0;
    }


    public XrayNodesViewModel(
        IXrayNodeSubService subService,
        IXrayNodeRepository xrayNodeRepository,
        IXrayNodeService xrayNodeService,
        IXrayNodeSubRepository xrayNodeSubRepository,
        IXrayService xrayService,
        IMapper mapper,
        IClipboard clipboard
        )
    {
        _subService = subService;
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
        var xraynodes = _xrayNodeRepository.GetList().OrderByDescending(o => o.Speed);
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
        OnResetCommand();
    }

    /// <summary>
    /// 更新订阅
    /// </summary>
    private async void OnUpdateNodeCommand()
    {
        if (IsProgress)
        {
            Warning("更新订阅被取消", $"等待{ProgressText}完成后再试");
            return;
        }
        var subs = _xrayNodeSubRepository.GetListByEnable();
        var total = subs.Count * 1d;
        if (total > 0)
        {
            ClearProgress();
            ProgressText = "更新订阅";
            IsProgress = true;
        }
        _progressCancel = () =>
        {
            ClearProgress();
            _subService.Cancel();
        };

        var nodeTotal = 0;
        for (var i = 0; i < total; i++)
        {
            if (!IsProgress)
            {
                break;
            }
            var sub = subs[i];
            var url = sub.ParseUrl();
            var nodes = await _subService.GetXrayNode(url);
            Success("更新订阅", $"{sub.Remark}更新了{nodes.Count}个节点");
            if (nodes.Count == 0)
            {
                continue;
            }
            nodeTotal += nodes.Count;
            _xrayNodeService.SaveNodes(nodes);
            ProgressValue = (int)((i + 1) / total * 100);
            OnResetCommand();
        }
        OnResetCommand();
        Success("节点订阅更新完成", $"共更新了{nodeTotal}个节点");
        await Task.Delay(1000);
        ClearProgress();
    }

    /// <summary>
    /// 测试所有节点速度
    /// </summary>
    private void OnSpeedTest()
    {
        if (IsProgress)
        {
            Warning("测速被取消", $"等待{ProgressText}完成后再试");
            return;
        }

        var items = XrayNodeItemsSource.Select(_mapper.Map<XrayNodeEntity>).ToList();
        var total = items.Count * 1d;
        if (total > 0)
        {
            ClearProgress();
            ProgressText = "检测节点";
            IsProgress = true;
        }
        var speedService = new XrayNodeSpeedTest(items, 10, 3000);
        _progressCancel = speedService.Cancel;
        speedService.OnProgress += (value) => ProgressValue = value;
        speedService.OnCancel += ClearProgress;
        speedService.OnTested += (result) =>
        {
            var entity = result.Entity;
            var model = XrayNodeItemsSource.FirstOrDefault(o => o.Url == entity.Url);
            if (model == null)
            {
                return;
            }

            if (result.IsSuccess)
            {
                model.Status = entity.Status = XrayNodeStatus.Success;
                model.Remark = entity.Remark = result.Remark;
                model.Speed = entity.Speed = result.Speed;
                model.Updated = DateTime.Now;
            }
            else
            {
                model.Status = entity.Status = XrayNodeStatus.Error;
                model.Remark = entity.Remark = "不可用";
                model.Speed = entity.Speed = 0;
            }
        };

        speedService.OnCompeleted += async () =>
        {
            _xrayNodeRepository.SaveNodes(items);
            await _xrayNodeRepository.DeleteByStatusError();
            OnResetCommand();
            await Task.Delay(1000);
            ClearProgress();
            Success("节点检测完成");
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
                var content = XrayUtils.Base64Decode(item);
                var nodes = content.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
                _xrayNodeService.SaveNodes(nodes);
            }
            else
            {
                _xrayNodeService.SaveNodes([item]);
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
