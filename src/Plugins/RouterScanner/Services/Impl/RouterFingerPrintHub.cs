namespace RouterScanner.Services.Impl;

public sealed class RouterFingerPrintHub : IRouterFingerPrintHub
{
    public List<RouterFingerPrintMatch> Matches { get; private set; } = [];

    public RouterFingerPrintHub(string? filepath = null)
    {
        filepath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "router-finger-print.txt");
        using var sr = File.OpenText(filepath);

        while (!sr.EndOfStream)
        {
            var lineTxt = sr.ReadLine();
            if (string.IsNullOrWhiteSpace(lineTxt) || lineTxt.StartsWith('#'))
            {
                continue;
            }

            if (MatchParse(lineTxt))
            {
                continue;
            }
        }
    }

    private bool MatchParse(string input)
    {
        RouterFingerPrintMatch match = new();
        var isOk = match.Parse(input);
        if (isOk)
        {
            Matches.Add(match);
        }
        return isOk;
    }
}

/// <summary>
/// 路由器指纹
/// </summary>
public sealed class RouterFingerPrintMatch
{
    /// <summary>
    /// 服务探针参数
    /// </summary>
    public string Pattern { get; set; } = string.Empty;
    /// <summary>
    /// 服务版本信息
    /// </summary>
    public RouterVersionInfo Info { get; set; } = new();

    public RouterFingerPrintMatch()
    {
    }
    public bool Parse(string input)
    {
        var pattern = "^match m[|](?<pattern>.*)[|].*";
        var reg = Regex.Match(input, pattern);
        if (!reg.Success)
        {
            return false;
        }
        Pattern = Regex.Unescape(reg.Result("${pattern}"));
        Info.Parse(input);
        return true;
    }
}

/// <summary>
/// 路由器版本信息
/// </summary>
public sealed class RouterVersionInfo
{
    /// <summary>
    /// 厂商|路由名称
    /// </summary>
    public string Production { get; set; } = string.Empty;
    /// <summary>
    /// 版本
    /// </summary>
    public string Version { get; set; } = string.Empty;
    /// <summary>
    /// 固件版本
    /// </summary>
    public string Firmware { get; set; } = string.Empty;

    public RouterVersionInfo()
    {
    }

    public void Parse(string input)
    {
        ProductionParse(input);
        VersionParse(input);
        FirmwareParse(input);
    }
    private void ProductionParse(string input)
    {
        var pattern = "p/(?<val>.*)/p";
        var reg = Regex.Match(input, pattern);
        if (!reg.Success)
        {
            return;
        }
        Production = reg.Result("${val}");
    }
    private void VersionParse(string input)
    {
        var pattern = "v/(?<val>.*)/v";
        var reg = Regex.Match(input, pattern);
        if (!reg.Success)
        {
            return;
        }
        Version = reg.Result("${val}");
    }
    private void FirmwareParse(string input)
    {
        var pattern = "f/(?<val>.*)/f";
        var reg = Regex.Match(input, pattern);
        if (!reg.Success)
        {
            return;
        }
        Version = reg.Result("${val}");
    }

}