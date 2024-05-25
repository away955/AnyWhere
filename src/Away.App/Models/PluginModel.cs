namespace Away.App.Models;

public sealed class PluginModel : ReactiveObject
{
    private string _currentVersion = string.Empty;
    private double _fileSize;

    /// <summary>
    /// 插件模块名称
    /// </summary>
    [Reactive]
    public string Module { get; set; } = string.Empty;
    /// <summary>
    /// 插件名称
    /// </summary>
    [Reactive]
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// 说明
    /// </summary>
    [Reactive]
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// 当前版本
    /// </summary>
    public string CurrentVersion
    {
        get => _currentVersion;
        set
        {
            this.RaiseAndSetIfChanged(ref _currentVersion, value);
            this.RaisePropertyChanged(nameof(IsInstalled));
            this.RaisePropertyChanged(nameof(IsUpgrade));
        }
    }
    /// <summary>
    /// 最新版本
    /// </summary>
    [Reactive]
    public string LatestVersion { get; set; } = string.Empty;
    /// <summary>
    /// 是否禁用
    /// </summary>
    [Reactive]
    public bool IsDisabled { get; set; }
    /// <summary>
    /// 文件编号
    /// </summary>
    [Reactive]
    public string ContentID { get; set; } = string.Empty;
    /// <summary>
    /// 图片|url
    /// </summary>
    [Reactive]
    public string Image { get; set; } = string.Empty;
    /// <summary>
    /// 图标
    /// </summary>
    [Reactive]
    public string Logo { get; set; } = string.Empty;
    /// <summary>
    /// 菜单地址
    /// </summary>
    [Reactive]
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 文件大小/kb
    /// </summary>
    public double FileSize
    {
        get => _fileSize;
        set
        {
            this.RaiseAndSetIfChanged(ref _fileSize, value);
            this.RaisePropertyChanged(nameof(FileSizeRemark));
        }
    }

    /// <summary>
    /// 已安装
    /// </summary>
    public bool IsInstalled => !string.IsNullOrWhiteSpace(CurrentVersion);
    /// <summary>
    /// 可升级
    /// </summary>
    public bool IsUpgrade => IsInstalled && CurrentVersion != LatestVersion;
    /// <summary>
    /// 文件大小说明
    /// </summary>
    public string FileSizeRemark
    {
        get
        {
            return FileSize switch
            {
                var i when 0 < i && i < 1024 => $"{Math.Round(i, 2)}KB",
                var i when 1024 < i && i < 1024 * 1024 => $"{Math.Round(i / 1024, 2)}MB",
                _ => string.Empty
            };
        }
    }

    [Reactive]
    public Bitmap? ImageSouce { get; set; }

    /// <summary>
    /// 安装中
    /// </summary>
    [Reactive]
    public bool Installing { get; set; }

    /// <summary>
    /// 卸载中
    /// </summary>
    [Reactive]
    public bool UnInstalling { get; set; }
}
