namespace Away.Service.DB.Impl;

public sealed class SugerDbContext : SqlSugarScope, ISugerDbContext
{
    public SugerDbContext(ConnectionConfig config) : base(config)
    {
        CodeFirst.InitTables<MenuEntity>();
        CodeFirst.InitTables<SettingsEntity>();
    }
}
