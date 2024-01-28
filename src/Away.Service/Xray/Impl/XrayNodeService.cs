namespace Away.Service.Xray.Impl;

[ServiceInject]
public class XrayNodeService : IXrayNodeService
{
    private readonly HttpClient _httpClient;
    public XrayNodeService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("xray");
    }

    public async Task<List<string>> GetXrayNodeByUrl(string url)
    {
        var response = await _httpClient.GetStringAsync(url);
        var items = response.Split("\r\n");
        return items.ToList();
    }
}
