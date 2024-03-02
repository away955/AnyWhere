namespace Away.Service.DB.Repositories.Impl;

[ServiceInject]
public class XrayNodeRepository(ISugerDbContext db) : RepositoryBase<XrayNodeEntity>(db), IXrayNodeRepository
{
    public Task<int> SaveNodes(List<XrayNodeEntity> entities)
    {
        return Context.Storageable(entities).WhereColumns(o => new { o.Host, o.Port }).ExecuteCommandAsync();
    }

    public bool DeleteNodesByLtTime(DateTime dateTime)
    {
        return this.Delete(o => o.Status == XrayNodeStatus.Error && o.Updated < dateTime);
    }
}
