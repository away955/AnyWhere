using Away.App.Domain.Setting.Entities;

namespace Away.App.Domain.Setting.Impl;

[DI]
public sealed class AppSettingRepository : IAppSettingRepository
{
    private readonly ISugarDbContext db;
    public AppSettingRepository(ISugarDbContext db)
    {
        this.db = db;
        db.CodeFirst.InitTables<AppSettingEntity>();
    }

    private ISimpleClient<AppSettingEntity>? _tb;
    private ISimpleClient<AppSettingEntity> TB => _tb ??= db.GetSimpleClient<AppSettingEntity>();


    public string Get(string key)
    {
        return TB.GetById(key)?.Value ?? string.Empty;
    }

    public bool Set(string key, string value)
    {
        return TB.InsertOrUpdate(new AppSettingEntity { Key = key, Value = value });
    }

    public bool Delete(string key)
    {
        return TB.DeleteById(key);
    }
}
