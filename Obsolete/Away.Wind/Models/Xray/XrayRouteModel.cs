namespace Away.Wind.Models;

public class XrayRouteModel : BindableBase
{
    private string _domainStrategy = "AsIs";
    private string _domainMatcher = "linear";

    public string domainStrategy
    {
        get => _domainStrategy;
        set => SetProperty(ref _domainStrategy, value);
    }
    public string domainMatcher
    {
        get => _domainMatcher;
        set => SetProperty(ref _domainMatcher, value);
    }

    private ObservableCollection<XrayRouteRuleModel> _rules = [];
    public ObservableCollection<XrayRouteRuleModel> rules
    {
        get => _rules;
        set => SetProperty(ref _rules, value);
    }
}

public class XrayRouteRuleModel : BindableBase
{
    private string _type = string.Empty;
    private string _outboundTag = string.Empty;

    public string type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }
    public string outboundTag
    {
        get => _outboundTag;
        set => SetProperty(ref _outboundTag, value);
    }
    public List<string> domain { get; set; } = [];
    public List<string> ip { get; set; } = [];
    public List<string> inboundTag { get; set; } = [];

    private string _domain = string.Empty;
    public string Domain
    {
        get => _domain;
        set => SetProperty(ref _domain, value);
    }

    private string _ip = string.Empty;
    public string IP
    {
        get => _ip;
        set => SetProperty(ref _ip, value);
    }

    private string _inboundTag = string.Empty;
    public string InboundTag
    {
        get => _inboundTag;
        set => SetProperty(ref _inboundTag, value);
    }
}