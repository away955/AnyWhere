namespace Away.App.Services;

/// <summary>
/// App 版本服务
/// </summary>
public interface IVersionService
{
    /// <summary>
    /// 获取App新版信息
    /// </summary>
    /// <returns></returns>
    Task<(AppResource?, AppResource?)> GetAppResource();
    /// <summary>
    /// 获取App下载地址
    /// </summary>
    /// <param name="contentID"></param>
    /// <returns></returns>
    Task<string> GetDownloadRequest(string contentID);
    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="contentID">文件编号</param>
    /// <param name="downloadFilePath">存储地址</param>
    /// <returns></returns>
    public Task<bool> DownloadFile(string contentID, string downloadFilePath);
}
