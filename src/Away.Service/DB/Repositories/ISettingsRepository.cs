namespace Away.Service.DB.Repositories;

public interface ISettingsRepository : IRepositoryBase<SettingsEntity>
{
    string? GetValue(string key);
}
