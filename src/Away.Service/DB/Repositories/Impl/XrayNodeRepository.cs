namespace Away.Service.DB.Repositories.Impl;

[ServiceInject]
public class XrayNodeRepository(ISugerDbContext db) : RepositoryBase<XrayNodeEntity>(db), IXrayNodeRepository
{
    public Task<int> SaveNodes(List<XrayNodeEntity> entities)
    {
        return Context.Storageable(entities).WhereColumns(o => new { o.Host, o.Port }).ExecuteCommandAsync();
    }
}
