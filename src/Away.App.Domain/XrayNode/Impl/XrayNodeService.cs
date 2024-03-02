namespace Away.Domain.XrayNode.Impl;

[DI(ServiceLifetime.Scoped)]
public class XrayNodeService(
    ILogger<XrayNodeService> logger,
    IHttpClientFactory httpClientFactory,
    IXrayNodeRepository xrayNodeRepository) : IXrayNodeService
{
    private readonly ILogger<XrayNodeService> _logger = logger;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("xray");
    private readonly IXrayNodeRepository _xrayNodeRepository = xrayNodeRepository;

    public async Task SetXrayNodeByUrl(string url)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(url);
            SetXrayNodeByBase64String(response);
        }
        catch (Exception ex)
        {
            _logger.LogInformation("{}", ex.Message);
        }
    }

    public void SetXrayNodeByBase64String(string text)
    {
        var items = XrayUtils.Base64Decode(text).Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
        SaveXrayNodeByList(items);
    }

    public void SaveXrayNodeByList(List<string> nodes)
    {
        var unknows = new HashSet<string>();
        var list = new List<XrayNodeEntity>();
        foreach (var item in nodes)
        {
            var vmess = Vmess.Parse(item);
            if (vmess != null)
            {
                list.Add(vmess.ToEntity());
                continue;
            }

            var shadowsocks = Shadowsocks.Parse(item);
            if (shadowsocks != null)
            {
                list.Add(shadowsocks.ToEntity());
                continue;
            }

            var trojan = Trojan.Parse(item);
            if (trojan != null)
            {
                list.Add(trojan.ToEntity());
                continue;
            }

            unknows.Add(item);
        }
        if (unknows.Count > 0)
        {
            _logger.LogWarning("未知类型\n\r{}", JsonSerializer.Serialize(unknows.ToArray()));
        }
        var entities = list.Where(o => !o.Alias.StartsWith("更新于"))
            .Select(o =>
            {
                var context = o.Alias.Split('-');
                if (context.Length > 1)
                {
                    o.Alias = context.LastOrDefault()?.Trim() ?? string.Empty;
                }
                return o;
            }).ToList();
        _xrayNodeRepository.SaveNodes(entities);
        Log.Information($"更新{entities.Count}个节点");
    }
}
