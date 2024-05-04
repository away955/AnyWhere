namespace Away.App.Domain.RouterScanner.Entities;

/// <summary>
/// 路由器漏洞表
/// </summary>
[SugarTable("router_vul")]
public sealed class RouterVulEntity
{
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }
}
