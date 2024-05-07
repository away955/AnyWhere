using Away.App.Services.Entities;
using System.Collections.Generic;

namespace Away.App.Services.Impl;

public sealed class PluginInstalledRepository : IPluginInstalledRepository
{
    private readonly ISugarDbContext _db;
    private readonly SimpleClient<PluginInstalledEntity> _tb;
    public PluginInstalledRepository([FromKeyedServices(Constant.DBKey)] ISugarDbContext db)
    {
        db.CodeFirst.InitTables<PluginInstalledEntity>();
        _db = db;
        _tb = db.GetSimpleClient<PluginInstalledEntity>();
    }

    public List<PluginInstalledEntity> GetList()
    {
        return _tb.GetList();
    }

    public List<PluginInstalledEntity> GetListByNotDisabled()
    {
        return _tb.AsQueryable().Where(o => o.IsDisabled == false).ToList();
    }

    public bool Save(PluginInstalledEntity entity)
    {
        return _tb.InsertOrUpdate(entity);
    }

    public bool Delete(string module)
    {
        return _tb.DeleteById(module);
    }
}
