using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Away.App.Services.Impl;

public sealed class PluginStoreService : PluginStoreYun139, IPluginStoreService
{
    /// <summary>
    /// 插件安装根地址
    /// </summary>
    private static readonly string RootPath = Constant.PluginsRootPath;
    /// <summary>
    /// 临时文件夹
    /// </summary>
    private static readonly string TempPath = Path.Combine(Constant.RootPath, "tmp");

    static PluginStoreService()
    {
        if (!Directory.Exists(RootPath))
        {
            Directory.CreateDirectory(RootPath);
        }
        if (!Directory.Exists(TempPath))
        {
            Directory.CreateDirectory(TempPath);
        }
    }

    private readonly IAppMenuRepository _appMenuRepository;
    private readonly IPluginStoreRepository _pluginStoreRepository;
    private readonly IAppMapper _mapper;

    public PluginStoreService(
        IHttpClientFactory httpClientFactory,
        IAppMenuRepository appMenuRepository,
        IPluginStoreRepository pluginStoreRepository,
        IAppMapper mapper) : base(httpClientFactory)
    {
        _appMenuRepository = appMenuRepository;
        _pluginStoreRepository = pluginStoreRepository;
        _mapper = mapper;
    }

    public List<PluginStoreModel> GetList()
    {
        return _pluginStoreRepository.GetList();
    }

    public async Task<bool> Install(PluginStoreModel model)
    {
        PluginRegisterManager.UnLoadPlugin(model.Module);

        // 下载插件包
        var pluginZipFile = Path.Combine(TempPath, $"{model.Module}.zip");
        var flag = await DownloadFile(model.ContentID, pluginZipFile);
        if (!flag)
        {
            return false;
        }

        // 解压安装
        using (var archive = ZipFile.Open(pluginZipFile, ZipArchiveMode.Read, System.Text.Encoding.Default))
        {
            archive.ExtractToDirectory(RootPath, true);
        }
        File.Delete(pluginZipFile);


        // 注册插件
        var entity = _mapper.Map<PluginRegisterEntity>(model);
        _pluginStoreRepository.Register(entity);

        // 加载插件
        PluginRegisterManager.LoadPlugin(model.Module);
        AwayLocator.Refresh();

        // 添加菜单
        AppMenuEntity menuEntity = new()
        {
            Icon = model.Logo,
            Path = model.Path,
            Module = model.Module,
            Title = model.Name
        };
        _appMenuRepository.Save(menuEntity);
        RefreshMenu();
        return true;
    }

    public Task<bool> Upgrade(PluginStoreModel model)
    {
        return Install(model);
    }

    public bool UnInstall(string module)
    {
        PluginRegisterManager.UnLoadPlugin(module);

        // 移除插件注册
        _pluginStoreRepository.UnRegister(module);

        // 移除菜单
        _appMenuRepository.DeleteByModule(module);
        RefreshMenu();
        return true;
    }

    public bool DisabledOrEnable(PluginStoreModel model)
    {
        // 禁用菜单
        _appMenuRepository.IsDisabledByModule(model.Module);
        RefreshMenu();

        // 禁用插件注册
        model.IsDisabled = !model.IsDisabled;
        var entity = _mapper.Map<PluginRegisterEntity>(model);
        return _pluginStoreRepository.Register(entity);
    }

    public async Task<bool> UpdateResource()
    {
        var resource = await GetResource();
        if (resource == null)
        {
            return false;
        }
        return _pluginStoreRepository.Save(resource.Plugins);
    }

    /// <summary>
    /// 刷新菜单
    /// </summary>
    private static void RefreshMenu()
    {
        MessageEvent.Run(new object(), "RefreshMenu");
    }
}


public abstract class PluginStoreYun139
{
    /// <summary>
    /// 移动云盘Token
    /// <br/><see cref="https://yun.139.com/"/>
    /// </summary>
    protected string Token { get; set; } = string.Empty;

    protected readonly IHttpClientFactory _httpClientFactory;

    public PluginStoreYun139(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _ = GetResource();
    }

    protected async Task<PluginStoreResource?> GetResource()
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
        var resource = YamlUtils.Deserialize<PluginStoreResource>(yaml);
        Token = resource.Authorization;
        return resource;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="contentID"></param>
    /// <param name="downloadFilePath"></param>
    /// <returns></returns>
    protected async Task<bool> DownloadFile(string contentID, string downloadFilePath)
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
    protected async Task<string> GetDownloadRequest(string contentID)
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
        var bytes = await resp.Content.ReadAsByteArrayAsync();
        return new Bitmap(new MemoryStream(bytes));
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

