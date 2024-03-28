namespace Away.App.Domain.Xray;

public interface IXrayNodeSubService
{
    Task<List<string>> GetXrayNode(string url);
    void Cancel();
}
