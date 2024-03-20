namespace Away.App.Domain.XrayNode;

/// <summary>
/// Xray 网络节点仓储
/// </summary>
public interface IXrayNodeRepository : IRepositoryBase<XrayNodeEntity>
{
    void DeleteByUrl(string url);
    Task DeleteByStatusError();
    Task Update(XrayNodeEntity entity);
    void SaveNodes(List<XrayNodeEntity> entities);
    void SetChecked(XrayNodeEntity entity);
}
