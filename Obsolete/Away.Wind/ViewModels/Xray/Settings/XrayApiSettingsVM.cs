namespace Away.Wind.ViewModels;

public class XrayApiSettingsVM : SettingsVMBase
{
    public XrayApiSettingsVM(IXrayService xrayService, IMapper mapper, IMessageService messageService)
        : base(xrayService, mapper, messageService)
    {
    }

    protected override void Init()
    {
        var config = _xrayService.Config.api ??= new XrayApi();
        tag = config.tag;

        var item = DefaultItems.Select(o =>
        {
            o.IsChecked = config.services.Any(oo => oo == o.Content);
            return o;
        });
        Items = new ObservableCollection<CheckBoxGroupModel>(item);
    }

    protected override void OnSaveCommand()
    {
        _xrayService.Config.api = new XrayApi
        {
            tag = tag,
            services = Items.Where(o => o.IsChecked).Select(o => o.Content).ToList(),
        };
        base.OnSaveCommand();
    }

    private string _tag = string.Empty;
    public string tag
    {
        get => _tag;
        set => SetProperty(ref _tag, value);
    }

    private ObservableCollection<CheckBoxGroupModel> _items = [];
    public ObservableCollection<CheckBoxGroupModel> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }
    private static readonly List<CheckBoxGroupModel> DefaultItems = [
        new CheckBoxGroupModel
        {
            Content = "HandlerService",
            ToolTip = "入站出站代理进行修改"
        },
        new CheckBoxGroupModel
        {
            Content = "LoggerService",
            ToolTip = "支持对内置 Logger 的重启，可配合 logrotate 进行一些对日志文件的操作"
        },
        new CheckBoxGroupModel
        {
            Content = "StatsService",
            ToolTip = "内置的数据统计服务"
        },
        new CheckBoxGroupModel
        {
            Content = "ReflectionService",
            ToolTip = "支持 gRPC 客户端获取服务端的 API 列表"
        }
    ];
}
