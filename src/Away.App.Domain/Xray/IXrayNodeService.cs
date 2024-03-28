namespace Away.App.Domain.Xray;

/// <summary>
/// 获取Xray网络节点
/// </summary>
public interface IXrayNodeService
{
    void SaveNodes(List<string> nodes);
}
