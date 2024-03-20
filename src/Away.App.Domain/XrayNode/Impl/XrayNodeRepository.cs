namespace Away.App.Domain.XrayNode.Impl;

[DI(ServiceLifetime.Singleton)]
public class XrayNodeRepository(IFileContext context)
    : RepositoryBase<XrayNodeEntity>(context), IXrayNodeRepository
{
    public void DeleteByUrl(string url)
    {
        Items.RemoveAll(x => x.Url == url);
        Save();
    }

    public Task DeleteByStatusError()
    {
        Items.RemoveAll(o => o.Status == XrayNodeStatus.Error);
        return SaveAsync();
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

    public void SetChecked(XrayNodeEntity entity)
    {
        foreach (var item in Items)
        {
            item.IsChecked = item.Url == entity.Url;
        }
        Save();
    }

    public Task Update(XrayNodeEntity entity)
    {
        var index = Items.FindIndex(o => o.Url == entity.Url);
        if (index == -1)
        {
            return Task.CompletedTask;
        }
        Items[index] = entity;
        return SaveAsync();
    }
}