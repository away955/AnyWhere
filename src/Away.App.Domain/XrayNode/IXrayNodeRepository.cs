namespace Away.App.Domain.XrayNode;

/// <summary>
/// Xray 网络节点仓储
/// </summary>
public interface IXrayNodeRepository : IRepositoryBase<XrayNodeEntity>
{
    void DeleteByUrl(string url);
    void DeleteByStatusError();
    void Update(XrayNodeEntity entity);
    void SaveNodes(List<XrayNodeEntity> entities);
}
