namespace Away.App.Services.Impl;

public sealed class VersionService : PluginStoreYun139, IVersionService
{
    public VersionService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<(AppResource?, AppResource?)> GetAppResource()
    {
        var resource = await GetResource();
        return (resource?.App, resource?.Updated);
    }
}
