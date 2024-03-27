﻿using Away.App.Domain.XrayNode.Entities;

namespace Away.App.Domain.XrayNode;

/// <summary>
/// 订阅节点仓储接口
/// </summary>
public interface IXrayNodeSubRepository
{
    List<XrayNodeSubEntity> GetList();
    List<XrayNodeSubEntity> GetListByEnable();
    bool DeleteById(int id);
    bool InsertOrUpdate(List<XrayNodeSubEntity> entities);
}
