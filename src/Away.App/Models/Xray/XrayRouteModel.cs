namespace Away.App.Models;

public sealed class XrayRouteModel : ViewModelBase
{
    [Reactive]
    public string domainStrategy { get; set; } = "AsIs";
    [Reactive]
    public string domainMatcher { get; set; } = "linear";

    [Reactive]
    public ObservableCollection<XrayRouteRuleModel> rules { get; set; } = [];
}

public sealed class XrayRouteRuleModel : ViewModelBase
{
    [Reactive]
    public string type { get; set; } = string.Empty;
    [Reactive]
    public string outboundTag { get; set; } = string.Empty;
   
    public List<string> domain { get; set; } = [];    
    public List<string> ip { get; set; } = [];
    public List<string> inboundTag { get; set; } = [];

    [Reactive]
    public string DomainStr { get; set; } = string.Empty;
    [Reactive]
    public string IPStr { get; set; } = string.Empty;
    [Reactive]
    public string InboundTagStr { get; set; } = string.Empty;
}