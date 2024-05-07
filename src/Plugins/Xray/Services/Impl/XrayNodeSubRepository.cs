﻿namespace Xray.Services.Impl;

internal class XrayNodeSubRepository : IXrayNodeSubRepository
{
    private readonly ISugarDbContext db;
    public XrayNodeSubRepository([FromKeyedServices(Constant.DBKey)] ISugarDbContext db)
    {
        this.db = db;
        db.CodeFirst.InitTables<XrayNodeSubEntity>();
    }

    private ISimpleClient<XrayNodeSubEntity>? _tb;
    private ISimpleClient<XrayNodeSubEntity> TB => _tb ??= db.GetSimpleClient<XrayNodeSubEntity>();

    public List<XrayNodeSubEntity> GetList()
    {
        return TB.GetList();
    }

    public List<XrayNodeSubEntity> GetListByEnable()
    {
        return TB.AsQueryable().Where(o => o.IsDisable == false).ToList();
    }

    public bool DeleteById(int id)
    {
        return TB.DeleteById(id);
    }

    public bool InsertOrUpdate(List<XrayNodeSubEntity> entities)
    {
        return TB.InsertOrUpdate(entities);
    }
}
