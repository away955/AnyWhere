using Away.App.Core.Database;
using Away.App.Core.Database.Impl;
using SqlSugar;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class SqlSugarServiceExtensions
{
    public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, string connectionString)
    {
        var db = new SugarDbContext(new ConnectionConfig()
        {
            DbType = DbType.Sqlite,
            ConnectionString = connectionString,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings()
            {
                SqliteCodeFirstEnableDescription = true //启用备注
            }
        });
        db.Aop.OnLogExecuting = (sql, args) =>
        {
            Log.Debug(sql);
        };
        services.AddSingleton<ISugarDbContext>(db);
        return services;
    }
}
