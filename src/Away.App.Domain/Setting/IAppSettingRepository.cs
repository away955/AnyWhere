namespace Away.App.Domain.Setting;

public interface IAppSettingRepository
{
    T? Get<T>(string key) where T : class, new();
    string Get(string key);
    bool Set<T>(string key, T value) where T : class, new();
    bool Set(string key, string value);
    bool Delete(string key);
}
