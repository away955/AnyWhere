namespace Away.App.Domain.Setting;

public interface IAppSettingRepository
{
    string Get(string key);
    bool Set(string key, string value);
    bool Delete(string key);
}
