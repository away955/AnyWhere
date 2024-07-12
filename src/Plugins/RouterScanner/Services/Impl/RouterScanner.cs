namespace RouterScanner.Services.Impl;

public sealed class RouterScanner : IRouterScanner
{
    private static readonly Dictionary<ParamType, string> IPAddressMatches = [];
    private static readonly Dictionary<ParamType, string> PortMatches = [];
    static RouterScanner()
    {
        IPAddressMatches.Add(ParamType.Region, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}[-]\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        IPAddressMatches.Add(ParamType.One, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");

        PortMatches.Add(ParamType.Region, @"\d{1,5}-\d{1,5}");
        PortMatches.Add(ParamType.One, @"\d{1,5}");
    }

    public event Action<FingerPrintResult>? OnFingerPrintCompleted;
    public event Action<VulResult>? OnVulCompleted;
    public event Action? OnCompleted;

    public string IPs { get; set; } = string.Empty;
    public string Ports { get; set; } = "80";
    public int FingerPrintThreads { get; set; } = 10;
    public int FingerPrintTimeout { get; set; } = 1000 * 3;
    public int VulThreads { get; set; } = 10;
    public int VulTimeout { get; set; } = 1000 * 3;
    public IWebProxy? Proxy { get; set; }

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }

    public void Run()
    {
        var hosts = ToHosts();
        hosts.AsParallel()
            .WithDegreeOfParallelism(FingerPrintThreads)
            .WithCancellation(_cancellationTokenSource.Token)
            .ForAll(Run);
        OnCompleted?.Invoke();
    }


    private async void Run(IPEndPoint host)
    {
        var fingerPrintScanner = AwayLocator.GetService<IRouterFingerPrintScanner>();
        fingerPrintScanner.Proxy = Proxy;
        fingerPrintScanner.Host = host;
        fingerPrintScanner.Timeout = FingerPrintTimeout;
        var res = await fingerPrintScanner.Run(_cancellationTokenSource.Token);
        OnFingerPrintCompleted?.Invoke(res);

        if (!res.Success || res.Result == null)
        {
            return;
        }
        var vulScanner = AwayLocator.GetService<IRouterVulScanner>();
        vulScanner.Threads = VulThreads;
        vulScanner.Timeout = VulTimeout;
        vulScanner.RouterInfo = res.Result;
        vulScanner.Url = res.Url;
        vulScanner.OnCompleted += OnVulCompleted;

        vulScanner.Run(_cancellationTokenSource.Token);
    }

    private List<IPEndPoint> ToHosts()
    {
        List<IPEndPoint> list = [];
        var ips = ToIPs();
        var ports = ToPorts();
        foreach (var port in ports)
        {
            var hosts = ips.Select(o => new IPEndPoint(o, port));
            list.AddRange(hosts);
        }
        return list;
    }

    private List<IPAddress> ToIPs()
    {
        List<IPAddress> addresses = [];
        foreach (var item in IPs.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var ips = IPParse(item);
            addresses.AddRange(ips);
        }
        return addresses;
    }

    private List<int> ToPorts()
    {
        List<int> list = [];
        foreach (var item in Ports.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var ips = PortParse(item);
            list.AddRange(ips);
        }
        return list;
    }

    private static List<IPAddress> IPParse(string text)
    {
        List<IPAddress> list = [];
        foreach (var (key, pattern) in IPAddressMatches)
        {
            var reg = Regex.Match(text, pattern);
            if (!reg.Success)
            {
                continue;
            }
            if (key == ParamType.One)
            {
                list.Add(IPAddress.Parse(text));
            }
            else if (key == ParamType.Region)
            {
                var arr = text.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length != 2)
                {
                    continue;
                }
                var start = IPAddressUtils.ToInt(arr.First());
                var end = IPAddressUtils.ToInt(arr.Last());
                var count = Math.Abs(end - start) + 1;
                var ips = Enumerable.Range(start, count).Select(IPAddressUtils.ToIPAddress);
                list.AddRange(ips);
            }
            break;
        }

        return list;
    }

    private static List<int> PortParse(string text)
    {
        List<int> list = [];
        foreach (var (key, pattern) in PortMatches)
        {
            var reg = Regex.Match(text, pattern);
            if (!reg.Success)
            {
                continue;
            }
            if (key == ParamType.One)
            {
                var port = Convert.ToInt32(text);
                if (IPEndPoint.MinPort > port && port > IPEndPoint.MaxPort)
                {
                    continue;
                }
                list.Add(port);
            }
            else if (key == ParamType.Region)
            {
                var arr = text.Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length != 2)
                {
                    continue;
                }
                var start = Convert.ToInt32(arr.First());
                if (IPEndPoint.MinPort > start || start > IPEndPoint.MaxPort)
                {
                    continue;
                }
                var end = Convert.ToInt32(arr.Last());
                if (IPEndPoint.MinPort > end || end > IPEndPoint.MaxPort)
                {
                    continue;
                }
                if (end - start < 0)
                {
                    continue;
                }
                var count = Math.Abs(end - start) + 1;
                var ips = Enumerable.Range(start, count);
                list.AddRange(ips);
            }
            break;
        }

        return list;
    }


    /// <summary>
    /// 参数类型
    /// </summary>
    private enum ParamType
    {
        /// <summary>
        /// 单个值
        /// </summary>
        One,
        /// <summary>
        /// 区间值
        /// </summary>
        Region
    }
}
