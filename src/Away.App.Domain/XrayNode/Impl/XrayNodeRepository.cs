namespace Away.App.Domain.XrayNode.Impl;

[DI(ServiceLifetime.Scoped)]
public class XrayNodeRepository(IFileContext context)
    : RepositoryBase<XrayNodeEntity>(context), IXrayNodeRepository
{
    public void DeleteByUrl(string url)
    {
        Items.RemoveAll(x => x.Url == url);
        Save();
    }

    public void DeleteByStatusError()
    {
        Items.RemoveAll(o => o.Status == XrayNodeStatus.Error);
        Save();
    }

    public void SaveNodes(List<XrayNodeEntity> entities)
    {
        foreach (var entity in entities)
        {
            var index = Items.FindIndex(x => x.Host == entity.Host && x.Port == entity.Port);
            if (index > -1)
            {
                Items[index] = entity;
                continue;
            }
            Items.Add(entity);
        }
        Save();
    }

    public void Update(XrayNodeEntity entity)
    {
        var index = Items.FindIndex(o => o.Url == entity.Url);
        if (index == -1)
        {
            return;
        }
        Items[index] = entity;
        Save();
    }
}