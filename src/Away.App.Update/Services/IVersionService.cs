namespace Away.App.Services;

/// <summary>
/// App 版本服务
/// </summary>
public interface IVersionService
{
    Task<VersionInfo> GetVersionInfo(string url);
}
