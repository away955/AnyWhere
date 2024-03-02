namespace Away.Domain.XrayNode;

/// <summary>
/// 获取Xray网络节点
/// </summary>
public interface IXrayNodeService
{
    Task SetXrayNodeByUrl(string url);
    void SetXrayNodeByBase64String(string text);
    void SaveXrayNodeByList(List<string> nodes);
}
