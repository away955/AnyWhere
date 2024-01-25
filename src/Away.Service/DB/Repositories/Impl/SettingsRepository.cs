namespace Away.Service.DB.Repositories.Impl;

public class SettingsRepository(ISugerDbContext db) : RepositoryBase<SettingsEntity>(db), ISettingsRepository
{
    public string? GetValue(string key)
    {
        return this.AsQueryable().Where(o => o.Key == key).First()?.Value;
    }
}
