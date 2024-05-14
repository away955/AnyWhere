namespace Away.App.Services;

/// <summary>
/// App 升级服务
/// </summary>
public interface IUpgradeService
{
    /// <summary>
    /// 下载进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnDownloadProgress;
    /// <summary>
    /// 安装进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnInstallProgress;
    /// <summary>
    /// 更新错误
    /// </summary>
    public event Action<string>? OnError;

    Task Start(string url);
    void Cancel();
}
