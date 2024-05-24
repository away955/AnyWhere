namespace Away.App.ViewModels;

public sealed class PluginStoreViewModel : ViewModelBase
{
    private readonly IPluginStoreService _pluginStoreService;
    private readonly IAppMapper _appMapper;

    /// <summary>
    /// 刷新
    /// </summary>
    public ICommand ResetCommand { get; }
    /// <summary>
    /// 更新插件资源
    /// </summary>
    public ICommand UpdateResourceCommand { get; }
    /// <summary>
    /// 安装
    /// </summary>
    public ICommand InstallCommand { get; }
    /// <summary>
    /// 卸载
    /// </summary>
    public ICommand UnInstallCommand { get; }
    /// <summary>
    /// 升级
    /// </summary>
    public ICommand UpgradeCommand { get; }
    /// <summary>
    /// 启用|禁用
    /// </summary>
    public ICommand IsDisabledCommand { get; }

    [Reactive]
    public ObservableCollection<PluginModel> Stores { get; private set; } = [];


    public PluginStoreViewModel(IPluginStoreService pluginStoreService, IAppMapper appMapper)
    {
        _appMapper = appMapper;
        _pluginStoreService = pluginStoreService;
        ResetCommand = ReactiveCommand.Create(Init);
        UpdateResourceCommand = ReactiveCommand.Create(OnUpdateResource);
        InstallCommand = ReactiveCommand.Create<PluginModel>(OnInstall);
        UnInstallCommand = ReactiveCommand.Create<PluginModel>(OnUnInstall);
        UpgradeCommand = ReactiveCommand.Create<PluginModel>(OnUpgrade);
        IsDisabledCommand = ReactiveCommand.Create<PluginModel>(OnIsDisabled);
        OnUpdateResource();
    }

    private async void OnUpdateResource()
    {
        var res = await _pluginStoreService.UpdateResource();
        if (res)
        {
            _ = Init();
            MessageShow.Success("插件资源更新成功");
        }
        else
        {
            MessageShow.Error("插件资源更新失败");
        }
    }

    private void OnIsDisabled(PluginModel model)
    {
        var plugin = _appMapper.Map<PluginStoreModel>(model);
        var res = _pluginStoreService.DisabledOrEnable(plugin);
        if (res)
        {
            Init();
            MessageShow.Success($"{model.Name} 操作成功");
        }
        else
        {
            MessageShow.Error($"{model.Name} 操作失败");
        }
    }

    private async void OnUpgrade(PluginModel model)
    {
        var plugin = _appMapper.Map<PluginStoreModel>(model);
        var res = await _pluginStoreService.Upgrade(plugin);
        if (res)
        {
            _ = Init();
            MessageShow.Success($"{model.Name} 升级成功");
        }
        else
        {
            MessageShow.Error($"{model.Name} 升级失败");
        }
    }

    private void OnUnInstall(PluginModel model)
    {
        var res = _pluginStoreService.UnInstall(model.Module);
        if (res)
        {
            Init();
            MessageShow.Success($"{model.Name} 卸载成功");
        }
        else
        {
            MessageShow.Error($"{model.Name} 卸载失败");
        }
    }

    private async void OnInstall(PluginModel model)
    {
        var plugin = _appMapper.Map<PluginStoreModel>(model);
        var res = await _pluginStoreService.Install(plugin);
        if (res)
        {
            _ = Init();
            MessageShow.Success($"{model.Name} 安装成功");
        }
        else
        {
            MessageShow.Error($"{model.Name} 安装失败");
        }
    }

    protected override void OnActivation()
    {
        Init();
    }

    private Task Init()
    {
        var items = _pluginStoreService.GetList().Select(_appMapper.Map<PluginModel>);
        Stores = new ObservableCollection<PluginModel>(items);

        Task.Run(async () =>
        {
            foreach (var item in Stores)
            {
                if (item.ImageSouce != null)
                {
                    continue;
                }
                item.ImageSouce = await _pluginStoreService.GetImgae(item.Image);
            }
        });
        return Task.CompletedTask;
    }

}
