using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Away.App.Update.Services.Impl;

public sealed class UpdateService : IUpdateService
{
    private const string DownLoadUrl = "";
    private readonly HttpClient _httpClient;

    private readonly CancellationTokenSource _cts = new();
    private string _basePath => AppDomain.CurrentDomain.BaseDirectory;
    private string _destinationPath => _basePath;
    private readonly string _zipPath;

    public UpdateService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("xray");
        Uri uri = new(DownLoadUrl);
        var filename = HttpUtility.UrlDecode(uri.Segments.Last());
        _zipPath = Path.Combine(_basePath, filename);
    }


    /// <summary>
    /// 下载进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnDownloadProgress;

    /// <summary>
    /// 安装进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnInstallProgress;

    public async Task Start()
    {
        await Download();
        Install();
    }

    public void Cancel()
    {
        _cts.Cancel();
    }

    private async Task Download()
    {
        using var stream = await _httpClient.GetStreamAsync(DownLoadUrl, _cts.Token);
        if (stream == null)
        {
            return;
        }

        using var fileStream = File.OpenWrite(_zipPath);

        long count = 0;
        long total = stream.Length;
        byte[] buffer = new byte[1024];
        while ((count = await stream.ReadAsync(buffer)) > 0)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }
            OnDownloadProgress?.Invoke(new UpdatelEventArgs
            {
                Description = $"{ToMebibyte(count)}M/{ToMebibyte(total)}M",
                ProgressValue = (int)(count * 1d / total * 100)
            });
            await fileStream.WriteAsync(buffer);
        }
    }

    private void Install()
    {
        using ZipArchive archive = ZipFile.OpenRead(_zipPath);
        archive.ExtractToDirectory(_destinationPath, true);
        int count = 1;
        int total = archive.Entries.Count;
        foreach (var entry in archive.Entries)
        {
            if (_cts.IsCancellationRequested)
            {
                break;
            }

            OnInstallProgress?.Invoke(new UpdatelEventArgs
            {
                Description = $"正在更新 {entry.FullName}",
                ProgressValue = (int)(count * 1d / total * 100)
            });
            entry.ExtractToFile(Path.Combine(_destinationPath, entry.FullName));
            count++;
        }
        File.Delete(_zipPath);
    }

    static double ToMebibyte(long num) => num * 1d / 1024 / 1024;
}

public sealed class UpdatelEventArgs
{
    public string Description { get; set; } = string.Empty;
    public int ProgressValue { get; set; }
}
