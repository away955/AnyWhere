namespace Away.App.PluginDomain.Services;

/// <summary>
/// App配置服务
/// </summary>
public interface IAppSettingService
{
    T? Get<T>(string key) where T : class, new();
    string Get(string key);
    bool Set<T>(string key, T value) where T : class, new();
    bool Set(string key, string value);
    bool Delete(string key);
}
