namespace Away.App.Domain.XrayNodeSub;

[DI(ServiceLifetime.Scoped)]
public class XrayNodeSubRepository(IFileContext context)
    : RepositoryBase<XrayNodeSubEntity>(context), IXrayNodeSubRepository
{
    public List<XrayNodeSubEntity> GetListByEnable()
    {
        return Items.Where(o => o.IsDisable == false).ToList();
    }
}
