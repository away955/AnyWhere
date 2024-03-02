namespace Away.Service.XrayNode;

/// <summary>
/// 获取Xray网络节点
/// </summary>
public interface IXrayNodeService
{
    Task SetXrayNodeByUrl(string url);
    Task SetXrayNodeByBase64String(string text);
    Task SaveXrayNodeByList(List<string> nodes);
}
