using Away.Service.XrayNode.Model;

namespace Away.Service.XrayNode.Impl;

[ServiceInject]
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
        var response = await _httpClient.GetStringAsync(url);
        await SetXrayNodeByBase64String(response);
    }

    public async Task SetXrayNodeByBase64String(string text)
    {
        var items = XrayUtils.Base64Decode(text).Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();
        await SaveXrayNodeByList(items);
    }

    public async Task SaveXrayNodeByList(List<string> nodes)
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
        var entities = list.Where(o => !o.Alias.StartsWith("更新于")).ToList();
        var res = await _xrayNodeRepository.SaveNodes(entities);
        Log.Information($"更新{res}个节点");
    }
}
