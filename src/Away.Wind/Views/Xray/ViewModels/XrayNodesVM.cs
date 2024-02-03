using AutoMapper;
using Away.Service.DB.Entities;
using Away.Service.Xray;
using Away.Service.XrayNode;
using Away.Service.XrayNode.Model;
using Away.Wind.Views.Xray.Models;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace Away.Wind.Views.Xray;

public class XrayNodesVM : BindableBase, INavigationAware
{
    private readonly ILogger<XrayNodesVM> _logger;
    private readonly IXrayNodeRepository _xrayNodeRepository;
    private readonly IXrayNodeService _xrayNodeService;
    private readonly IXrayService _xrayService;
    private readonly IMapper _mapper;

    public XrayNodesVM(
        ILogger<XrayNodesVM> logger,
        IXrayNodeRepository xrayNodeRepository,
        IXrayNodeService xrayNodeService,
        IXrayService xrayService,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _logger = logger;
        _xrayNodeRepository = xrayNodeRepository;
        _xrayNodeService = xrayNodeService;
        _xrayService = xrayService;

        ResetCommand = new(OnResetCommand);
        UpdateNodeCommand = new(OnUpdateNodeCommand);
        CheckedCommand = new(OnCheckedCommand);
        CopyCommand = new(OnCopyCommand);
        PasteCommand = new(OnPasteCommand);


    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        OnResetCommand();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext)
    {
        OnResetCommand();
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext)
    {

    }


    public DelegateCommand ResetCommand { get; private set; }
    private void OnResetCommand()
    {
        IsOpenXray = _xrayService.IsOpened;
        var xraynodes = _xrayNodeRepository.GetList();
        var items = xraynodes.Select(_mapper.Map<XrayNodeModel>);
        XrayNodeItemsSource = new ObservableCollection<XrayNodeModel>(items);
    }


    private bool _isOpenXray;
    public bool IsOpenXray
    {
        get => _isOpenXray;
        set
        {
            SetProperty(ref _isOpenXray, value);
            if (_isOpenXray)
            {
                _xrayService.XrayStart();
            }
            else
            {
                _xrayService.XrayClose();
            }
        }
    }

    private ObservableCollection<XrayNodeModel> _xrayNodeItemsSource = [];
    public ObservableCollection<XrayNodeModel> XrayNodeItemsSource
    {
        get => _xrayNodeItemsSource;
        set => SetProperty(ref _xrayNodeItemsSource, value);
    }

    public DelegateCommand UpdateNodeCommand { get; private set; }
    public async void OnUpdateNodeCommand()
    {
        await _xrayNodeService.SetXrayNodeByUrl("https://bulinkbulink.com/freefq/free/master/v2");
        OnResetCommand();
    }

    /// <summary>
    /// 选择代理节点
    /// </summary>
    public DelegateCommand<XrayNodeModel?> CheckedCommand { get; private set; }
    private void OnCheckedCommand(XrayNodeModel? entity)
    {
        if (entity == null || string.IsNullOrWhiteSpace(entity.Url))
        {
            return;
        }

        if ("vmess" == entity.Type)
        {
            var vmess = Vmess.Parse(entity.Url);
            SetProxy(o =>
            {
                o.SetOutbound(vmess!);
            });
        }
        else if ("trojan" == entity.Type)
        {
            var trojan = Trojan.Parse(entity.Url);
            SetProxy(o =>
            {
                o.SetOutbound(trojan!);
            });
        }
        else if ("shadowsocks" == entity.Type)
        {
            var model = Shadowsocks.Parse(entity.Url);
            SetProxy(o =>
            {
                o.SetOutbound(model!);
            });
        }

        // 设置选中节点
        foreach (var item in XrayNodeItemsSource)
        {
            item.IsChecked = item.Id == entity.Id;
        }
        var items = XrayNodeItemsSource.Select(_mapper.Map<XrayNodeEntity>).ToList();
        _xrayNodeRepository.SaveNodes(items);
    }
    private void SetProxy(Action<XrayConfig> action)
    {
        action(_xrayService.Config);
        _xrayService.SaveConfig();
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

public class XrayNodeStatusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            XrayNodeStatus.Error => "AirplaneAlert",
            XrayNodeStatus.Default => "AirplaneSearch",
            XrayNodeStatus.Success => "AirplaneCheck",
            _ => string.Empty
        }; ;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}