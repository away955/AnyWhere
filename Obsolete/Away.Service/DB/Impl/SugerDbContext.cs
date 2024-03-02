using System.Reflection;

namespace Away.Service.DB.Impl;

public sealed class SugerDbContext : SqlSugarScope, ISugerDbContext
{   
    public SugerDbContext(ConnectionConfig config) : base(config)
    {       
        var tables = typeof(SugerDbContext).Assembly.DefinedTypes.Where(o => o.GetCustomAttribute<SugarTable>() != null);
        CodeFirst.InitTables(tables.ToArray());
    }
}
