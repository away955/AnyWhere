using Away.App.Update.Services.Impl;
using System.Threading.Tasks;

namespace Away.App.Update.Services;

public interface IUpdateService
{
    /// <summary>
    /// 下载进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnDownloadProgress;

    /// <summary>
    /// 安装进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnInstallProgress;
    Task Start();
    void Cancel();
}
