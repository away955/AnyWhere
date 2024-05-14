namespace Away.App.Services.Impl;

public sealed class AppSettingService : IAppSettingService
{
    private readonly ISugarDbContext db;
    public AppSettingService([FromKeyedServices(Constant.DBKey)] ISugarDbContext db)
    {
        this.db = db;
        db.CodeFirst.InitTables<AppSettingEntity>();
    }

    private ISimpleClient<AppSettingEntity>? _tb;
    private ISimpleClient<AppSettingEntity> TB => _tb ??= db.GetSimpleClient<AppSettingEntity>();

    public T? Get<T>(string key) where T : class, new()
    {
        var json = Get(key);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }
        return JsonUtils.Deserialize<T>(json);
    }

    public bool Set<T>(string key, T value) where T : class, new()
    {
        if (value == null)
        {
            return false;
        }
        var json = JsonUtils.Serialize(value);
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }
        return Set(key, json);
    }

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
