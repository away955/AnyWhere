namespace Away.App.Domain.XrayNode;

public interface IXrayNodeSubService
{
    Task<List<string>> GetXrayNode(string url);
    void Cancel();
}
