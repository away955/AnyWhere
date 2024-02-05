namespace Away.Wind.Views.Xray.Models;

public class XrayLogModel : BindableBase
{
    private string _access = string.Empty;
    public string access { get => _access; set => SetProperty(ref _access, value); }

    private string _error = string.Empty;
    public string error { get => _error; set => SetProperty(ref _error, value); }

    private string _logLevel = string.Empty;
    public string loglevel { get => _logLevel; set => SetProperty(ref _logLevel, value); }

    private bool _dnsLog;
    public bool dnsLog { get => _dnsLog; set => SetProperty(ref _dnsLog, value); }
}
