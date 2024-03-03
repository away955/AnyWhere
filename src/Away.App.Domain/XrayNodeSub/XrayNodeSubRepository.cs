namespace Away.App.Domain.XrayNodeSub;

[DI(ServiceLifetime.Scoped)]
public class XrayNodeSubRepository(IFileContext context)
    : RepositoryBase<XrayNodeSubEntity>(context), IXrayNodeSubRepository
{
    public void DeleteByUrl(string url)
    {
        Items.RemoveAll(o => o.Url == url);
        Save();
    }

    public List<XrayNodeSubEntity> GetListByEnable()
    {
        return Items.Where(o => o.IsDisable == false).ToList();
    }

    public void Save(List<XrayNodeSubEntity> entities)
    {
        Items = entities;
        Save();
    }
}
