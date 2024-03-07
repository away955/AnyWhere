namespace Away.App.Domain.XrayNodeSub;

public interface IXrayNodeSubService
{
    Task<List<string>> GetXrayNode(string url);
    void Cancel();
}
