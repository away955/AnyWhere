namespace Away.App.Domain.RouterScanner.Impl;

[DI(ServiceLifetime.Transient)]
public sealed class RouterFingerPrintScanner(IRouterFingerPrintHub probeHub) : IRouterFingerPrintScanner
{
    public IWebProxy? Proxy { get; set; }
    public IPEndPoint Host { get; set; } = null!;
    public int Timeout { get; set; } = 1000 * 30;

    private List<RouterFingerPrintMatch> _matches => probeHub.Matches;

    public async ValueTask<FingerPrintResult> Run(CancellationToken cancellationToken = default)
    {
        FingerPrintResult res;
        var url = Host.Port == 80 ? $"http://{Host.Address}" : $"http://{Host}";
        try
        {
            res = await Run(url, cancellationToken).ConfigureAwait(false);
        }
        catch (TaskCanceledException)
        {
            res = FingerPrintResult.OK(false, url, "任务取消");
        }
        catch (HttpRequestException)
        {
            var url2 = Host.Port == 80 || Host.Port == 443 ? $"https://{Host.Address}" : $"https://{Host}";
            try
            {
                Log.Warning($"路由器指纹扫描：{Host} 切换https重试");
                res = await Run(url2, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                res = FingerPrintResult.OK(false, url2, ex.Message);
            }
        }

        return res;
    }

    private async ValueTask<FingerPrintResult> Run(string url, CancellationToken cancellationToken = default)
    {
        using var http = HttpClientUtils.CreateHttpClient(Proxy);
        http.Timeout = TimeSpan.FromMilliseconds(Timeout);
        var resp = await http.GetAsync(url, cancellationToken);
        resp.EnsureSuccessStatusCode();

        var headers = new List<string>();
        foreach (var header in resp.Headers)
        {
            var key = header.Key;
            var value = string.Join(';', header.Value);
            headers.Add($"{key}: {value}");
        }
        var headerStr = string.Join('\n', headers);
        var content = await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var text = $"{headerStr}\n\n{content}\n\n";

        foreach (var match in _matches)
        {
            var reg = new Regex(match.Pattern, RegexOptions.IgnoreCase);
            var res = reg.Match(text);
            if (res.Success)
            {
                return FingerPrintResult.OK(true, url, string.Empty, match.Info);
            }
        }
        return FingerPrintResult.OK(false, url);
    }
}
