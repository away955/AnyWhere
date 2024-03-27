using SqlSugar;

namespace Away.App.Core.Database.Impl;

public sealed class SugarDbContext : SqlSugarScope, ISugarDbContext
{
    public SugarDbContext(ConnectionConfig config) : base(config)
    {
    }
}