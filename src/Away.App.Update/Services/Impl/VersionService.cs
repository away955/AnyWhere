using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Away.App.Update.Services.Impl;

public sealed class VersionService(IHttpClientFactory httpClientFactory) : IVersionService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("xray");

    public async Task<VersionInfo> GetVersionInfo(string url)
    {
        var text = await _httpClient.GetStringAsync(url);
        var pattern = @"^# 哪都通 \((?<updated>\d{4}-\d{2}-\d{2})\)\n## 更新功能 v(?<version>.*.)\n";
        var reg = Regex.Match(text, pattern);
        if (!reg.Success)
        {
            return new VersionInfo();
        }
        var updated = reg.Result("${updated}");
        var version = reg.Result("${version}");
        var info = text.Replace(reg.Value, string.Empty);
        return new VersionInfo
        {
            Updated = updated,
            Version = version,
            Info = info
        };
    }
}

public sealed class VersionInfo
{
    public string Updated { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Info { get; set; } = string.Empty;

    private int _versionNumber;
    public int VersionNumber
    {
        get
        {
            if (_versionNumber == 0)
            {
                _versionNumber = ParseVersion(Version);
            }
            return _versionNumber;
        }
    }

    public bool HasNewVersion(string currentVersion)
    {
        return VersionNumber > ParseVersion(currentVersion);
    }

    private int ParseVersion(string ver)
    {
        if (string.IsNullOrWhiteSpace(ver))
        {
            return 0;
        }
        var arr = ver.Split('.', StringSplitOptions.TrimEntries);
        if (arr.Length != 3)
        {
            return 0;
        }
        return Convert.ToInt32(arr[0]) * 100000
            + Convert.ToInt32(arr[1]) * 1000
            + Convert.ToInt32(arr[2]) * 10;
    }
}
