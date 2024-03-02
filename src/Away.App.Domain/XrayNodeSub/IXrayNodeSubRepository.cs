﻿namespace Away.App.Domain.XrayNodeSub;

/// <summary>
/// 订阅节点仓储接口
/// </summary>
public interface IXrayNodeSubRepository : IRepositoryBase<XrayNodeSubEntity>
{
    List<XrayNodeSubEntity> GetListByEnable();
}
