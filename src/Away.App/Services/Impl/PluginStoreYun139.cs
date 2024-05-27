namespace Away.App.Services.Impl;

/// <summary>
/// 移动云盘操作类
/// <br/><see cref="https://yun.139.com/"/>
/// </summary>
public abstract class PluginStoreYun139
{
    /// <summary>
    /// 移动云盘Token
    /// </summary>
    protected string Token { get; set; } = string.Empty;

    protected readonly IHttpClientFactory _httpClientFactory;

    public PluginStoreYun139(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected async Task<StoreResource?> GetResource()
    {
#if DEBUG
        await Task.CompletedTask;
        var yaml = File.ReadAllText("D:\\git\\away-anywhere\\dist\\latest\\pluins.yml");
#else
        using var http = HttpClientUtils.CreateHttpClient();
        var resp = await http.GetAsync(Constant.PluginsStoreResource);
        if (!resp.IsSuccessStatusCode)
        {
            return null;
        }
        var yaml = await resp.Content.ReadAsStringAsync();
#endif
        var resource = YamlUtils.Deserialize<StoreResource>(yaml);
        Token = resource.Authorization;
        return resource;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="contentID"></param>
    /// <param name="downloadFilePath"></param>
    /// <returns></returns>
    public async Task<bool> DownloadFile(string contentID, string downloadFilePath)
    {
        var url = await GetDownloadRequest(contentID);
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        using var http = _httpClientFactory.CreateClient();
        SetHttpHeader(http);
        using var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!resp.IsSuccessStatusCode)
        {
            Log.Error($"下载插件失败：{resp.StatusCode}");
            return false;
        }
        using var fs = File.Create(downloadFilePath);
        await resp.Content.CopyToAsync(fs);
        return true;
    }

    /// <summary>
    /// 获取下载地址
    /// </summary>
    /// <param name="contentID"></param>
    /// <returns></returns>
    public async Task<string> GetDownloadRequest(string contentID)
    {
        var json = "{\"contentID\":\"" + contentID + "\",\"operation\":\"0\",\"inline\":\"0\",\"extInfo\":{\"isReturnCdnDownloadUrl\":\"1\"},\"commonAccountInfo\":{\"account\":\"18376911997\",\"accountType\":1}}";

        using var http = _httpClientFactory.CreateClient();
        var resp = await http.SendAsync($"""
            POST /orchestration/personalCloud/uploadAndDownload/v1.0/downloadRequest HTTP/1.1
            Host: yun.139.com
            Content-Type: application/json
            Authorization: {Token}

            {json}

            """, true);
        var jsonNode = await resp.Content.ReadFromJsonAsync<JsonNode>();
        if (jsonNode!["success"]!.ToString() == "false")
        {
            return string.Empty;
        }
        return jsonNode!["data"]!["downloadURL"]!.ToString();
    }

    public async Task<Bitmap?> GetImgae(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        using var http = _httpClientFactory.CreateClient();
        SetHttpHeader(http);
        var resp = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
        if (!resp.IsSuccessStatusCode)
        {
            Log.Error($"加载插件图片失败：{resp.StatusCode}");
            return null;
        }
        using var stream = new MemoryStream();
        await resp.Content.CopyToAsync(stream);
        return new Bitmap(stream);
    }

    protected async Task<List<DiskModel>> GetDisk()
    {
        var json = "{\"catalogID\":\"0y11YNYhH0VT13120240506210852wba\",\"sortDirection\":1,\"startNumber\":1,\"endNumber\":100,\"filterType\":0,\"catalogSortType\":0,\"contentSortType\":0,\"commonAccountInfo\":{\"account\":\"18376911997\",\"accountType\":1}}";

        using var http = _httpClientFactory.CreateClient();
        var resp = await http.SendAsync($"""
            POST /orchestration/personalCloud/catalog/v1.0/getDisk HTTP/1.1
            Host: yun.139.com 
            Authorization: {Token}
            Content-Type: application/json
            Origin: https://yun.139.com

            {json}

            """);
        var jsonNode = await resp.Content.ReadFromJsonAsync<JsonNode>();
        if (jsonNode!["success"]!.ToString() == "false")
        {
            return [];
        }
        var jn = jsonNode!["data"]!["getDiskResult"]!["contentList"];
        return JsonSerializer.Deserialize<List<DiskModel>>(jn) ?? [];
    }

    private void SetHttpHeader(HttpClient http)
    {
        http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(Token);
        http.DefaultRequestHeaders.Host = "yun.139.com";
        http.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36 Edg/125.0.0.0");
    }

    public class DiskModel
    {
        /// <summary>
        /// 文件编号
        /// </summary>
        public required string contentID { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public required string contentName { get; set; }
        /// <summary>
        /// 文件大小/b
        /// </summary>
        public int contentSize { get; set; }
    }
}
