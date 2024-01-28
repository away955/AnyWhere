namespace Away.Service.Xray;

/// <summary>
/// 获取Xray网络节点
/// </summary>
public interface IXrayNodeService
{
    Task<List<string>> GetXrayNodeByUrl(string url);
}
