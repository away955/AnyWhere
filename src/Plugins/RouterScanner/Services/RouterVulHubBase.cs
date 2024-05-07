namespace RouterScanner.Services;

public abstract class RouterVulHubBase<T> where T : IRouterVulHub
{
    /// <summary>
    /// 网络代理
    /// </summary>
    public IWebProxy? Proxy { get; set; }
    /// <summary>
    /// 路由地址
    /// </summary>
    public required string Url { get; set; }
    /// <summary>
    /// 攻击参数
    /// </summary>
    public Payload Payload { get; set; } = new()
    {
        UserName = "admin",
        Password = "admin"
    };


    protected Uri Uri => new(Url);
    protected string Host => $"{Uri.Host}:{Uri.Port}";
    protected bool SSL => Uri.Scheme == "https";
    protected HttpClient Http => HttpClientUtils.CreateHttpClient(Proxy);

    public abstract ValueTask<VulResult> Exp(CancellationToken cancellationToken = default);

    public virtual ValueTask<VulResult> Poc(CancellationToken cancellationToken = default)
    {
        return Exp(cancellationToken);
    }

    protected string GetBasicAuth()
    {
        var base64Str = EncryptUtils.Base64Encode($"{Payload.UserName}:{Payload.Password}");
        return $"Basic {base64Str}";
    }

    protected VulResult OK(bool success = true, string message = "")
    {
        return new VulResult { Success = success, Message = message, Vul = (IRouterVulHub)this };
    }
}
