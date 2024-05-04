namespace Away.App.Domain.RouterScanner.Impl;

[DI(ServiceLifetime.Transient)]
public sealed class RouterVulScanner : IRouterVulScanner
{
    private static IEnumerable<IRouterVulHub> VULHUBS => AwayLocator.GetKeyedServices<IRouterVulHub>(Constant.VulHubKey);

    public event Action<VulResult>? OnCompleted;

    public IWebProxy? Proxy { get; set; }
    public int Threads { get; set; } = 10;
    public int Timeout { get; set; } = 1000 * 30;
    public string Url { get; set; } = null!;
    public RouterVersionInfo RouterInfo { get; set; } = null!;


    public void Run(CancellationToken cancellationToken = default)
    {
        VULHUBS
            .Where(o => o.Production == RouterInfo.Production && o.Version.Contains(RouterInfo.Version))
            .AsParallel()
            .WithDegreeOfParallelism(Threads)
            .WithCancellation(cancellationToken)
            .ForAll(o => Run(o, cancellationToken));
    }

    private async void Run(IRouterVulHub vulHub, CancellationToken cancellationToken = default)
    {
        try
        {
            vulHub.Url = Url;
            var res = await vulHub.Poc(cancellationToken);
            OnCompleted?.Invoke(res);
        }
        catch (Exception ex)
        {
            var msg = $"""
                不存在漏洞: {Url}
                错误: {ex.Message}                
                """;
            Log.Warning(ex, msg);

            OnCompleted?.Invoke(new VulResult
            {
                Vul = vulHub,
                Success = false,
                Message = ex.Message
            });
        }
    }
}
