namespace Xray.Services;

public interface IXrayNodeSubService
{
    Task<List<string>> GetXrayNode(string url);
    void Cancel();
}
