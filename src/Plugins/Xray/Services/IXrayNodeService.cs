namespace Xray.Services;

/// <summary>
/// 获取Xray网络节点
/// </summary>
public interface IXrayNodeService
{
    void SaveNodes(List<string> nodes);
}
