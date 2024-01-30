namespace Away.Service.DB.Repositories.Impl;

[ServiceInject]
public sealed class SettingsRepository(ISugerDbContext db) : RepositoryBase<SettingsEntity>(db), ISettingsRepository
{
    public string? GetValue(string key)
    {
        return this.AsQueryable().First(o => o.Key == key)?.Value;
    }
}
