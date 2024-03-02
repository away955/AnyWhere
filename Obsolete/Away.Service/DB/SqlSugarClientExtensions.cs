using Away.Service.DB.Impl;

namespace Microsoft.Extensions.DependencyInjection;

public static class SqlSugarClientExtensions
{
    public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, string connectionString)
    {
        var db = new SugerDbContext(new ConnectionConfig()
        {
            DbType = DbType.Sqlite,
            ConnectionString = connectionString,
            InitKeyType = InitKeyType.Attribute,
            MoreSettings = new ConnMoreSettings()
            {
                SqliteCodeFirstEnableDescription = true //启用备注
            }
        });
        services.AddSingleton<ISugerDbContext>(db);
        return services;
    }
}