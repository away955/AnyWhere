namespace Away.Service.DB.Repositories;

public interface ISettingsRepository : ISimpleClient<SettingsEntity>
{
    string? GetValue(string key);
}
