namespace Away.App.Models;

public class XrayRouteModel : ViewModelBase
{
    [Reactive]
    public string domainStrategy { get; set; } = "AsIs";
    [Reactive]
    public string domainMatcher { get; set; } = "linear";

    [Reactive]
    public ObservableCollection<XrayRouteRuleModel> rules { get; set; } = [];
}

public class XrayRouteRuleModel : ViewModelBase
{
    [Reactive]
    public string type { get; set; } = string.Empty;
    [Reactive]
    public string outboundTag { get; set; } = string.Empty;
   
    public List<string> domain { get; set; } = [];    
    public List<string> ip { get; set; } = [];
    public List<string> inboundTag { get; set; } = [];

    [Reactive]
    public string Domain { get; set; } = string.Empty;
    [Reactive]
    public string IP { get; set; } = string.Empty;
    [Reactive]
    public string InboundTag { get; set; } = string.Empty;
}