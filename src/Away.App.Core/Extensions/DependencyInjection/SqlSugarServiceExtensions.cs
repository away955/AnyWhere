using SqlSugar;

namespace Away.App.Core.Extensions.DependencyInjection;

public static class SqlSugarServiceExtensions
{
    public static IServiceCollection UseSqlSugar(this IServiceCollection services, Action<ConnectionConfig> configAction, Action<SqlSugarClient>? clientAction = null)
    {
        var config = new ConnectionConfig();
        configAction(config);
        services.AddSingleton<ISqlSugarClient>(s =>
        {
            if (clientAction == null)
            {
                return new SqlSugarScope(config);
            }
            return new SqlSugarScope(config, clientAction);
        });
        return services;
    }
}
