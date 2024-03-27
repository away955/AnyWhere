using Away.App.Core.Database;
using Away.App.Domain.XrayNode.Entities;
using SqlSugar;

namespace Away.App.Domain.XrayNode.Impl;

[DI(ServiceLifetime.Singleton)]
internal class XrayNodeRepository : IXrayNodeRepository
{
    private readonly ISugarDbContext db;
    public XrayNodeRepository(ISugarDbContext db)
    {
        this.db = db;
        db.CodeFirst.InitTables<XrayNodeEntity>();
    }

    private ISimpleClient<XrayNodeEntity>? _tb;
    private ISimpleClient<XrayNodeEntity> TB => _tb ??= db.GetSimpleClient<XrayNodeEntity>();

    public List<XrayNodeEntity> GetList()
    {
        return TB.GetList();
    }

    public bool DeleteById(int id)
    {
        return TB.DeleteById(id);
    }

    public async Task DeleteByStatusError()
    {
        await TB.AsDeleteable().Where(o => o.Status == XrayNodeStatus.Error).ExecuteCommandAsync();
    }

    public void SaveNodes(List<XrayNodeEntity> entities)
    {
        db.Storageable(entities).WhereColumns(o => new { o.Host, o.Port }).ExecuteCommand();
    }

    public void SetChecked(XrayNodeEntity entity)
    {
        TB.AsUpdateable()
            .SetColumns(o => new XrayNodeEntity { IsChecked = false })
            .Where(o => o.IsChecked == true)
            .ExecuteCommand();

        entity.IsChecked = true;
        TB.Update(entity);
    }

    public async Task Update(XrayNodeEntity entity)
    {
        await TB.UpdateAsync(entity);
    }
}