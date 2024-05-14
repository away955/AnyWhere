using System.Net.Http.Headers;

namespace Away.App.Services.Impl;

public sealed class PluginStoreService : IPluginStoreService
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
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// 移动云盘Token
    /// <br/><see cref="https://yun.139.com/"/>
    /// </summary>
    private string Token { get; set; } = string.Empty;

    public PluginStoreService(
        IHttpClientFactory httpClientFactory,
        IAppMenuRepository appMenuRepository,
        IPluginStoreRepository pluginStoreRepository,
        IAppMapper mapper)
    {
        _httpClientFactory = httpClientFactory;
        _appMenuRepository = appMenuRepository;
        _pluginStoreRepository = pluginStoreRepository;
        _mapper = mapper;

        _ = UpdateResource();
    }

    public List<PluginStoreModel> GetList()
    {
        return _pluginStoreRepository.GetList();
    }

    public async Task<bool> Install(PluginStoreModel model)
    {
        PluginRegisterManager.UnLoadPlugin(model.Module);
#if DEBUG
        await Task.CompletedTask;
#else
        Uri uri = new(model.DownloadUrl);
        var pluginZipFile = Path.Combine(TempPath, $"{model.Module}.zip");

        // 下载插件包
        using var http = _httpClientFactory.CreateClient();
        http.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(Token);
        using var resp = await http.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        if (!resp.IsSuccessStatusCode)
        {
            return false;
        }
        using (var fs = File.Create(pluginZipFile))
        {
            await resp.Content.CopyToAsync(fs);
        }

        // 解压安装
        using (var archive = ZipFile.Open(pluginZipFile, ZipArchiveMode.Read, System.Text.Encoding.Default))
        {
            archive.ExtractToDirectory(RootPath, true);
        }
        File.Delete(pluginZipFile);
#endif


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
#if DEBUG
        await Task.CompletedTask;
        var yaml = File.ReadAllText("D:\\git\\away-anywhere\\dist\\latest\\pluins.yml");
#else
        using var http = HttpClientUtils.CreateHttpClient();
        var resp = await http.GetAsync(Constant.PluginsStoreResource);
        if (!resp.IsSuccessStatusCode)
        {
            return false;
        }
        var yaml = await resp.Content.ReadAsStringAsync();
#endif
        var resource = YamlUtils.Deserialize<PluginStoreResource>(yaml);
        Token = resource.Authorization;
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


