namespace Away.Service.DB.Repositories;

/// <summary>
/// Xray 网络节点仓储
/// </summary>
public interface IXrayNodeRepository : IRepositoryBase<XrayNodeEntity>
{
    Task<int> SaveNodes(List<XrayNodeEntity> entities);

    bool DeleteNodesByLtTime(DateTime dateTime);
}
