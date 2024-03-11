using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Away.App.Update.Services.Impl;

public sealed class UpdateService(IHttpClientFactory httpClientFactory) : IUpdateService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("xray");

    private readonly CancellationTokenSource _cts = new();
    private static string _basePath => AppDomain.CurrentDomain.BaseDirectory;
    private string _destinationPath => _basePath;
    private string _zipPath => Path.Combine(_basePath, _filename);
    private string _filename = string.Empty;


    /// <summary>
    /// 下载进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnDownloadProgress;

    /// <summary>
    /// 安装进度
    /// </summary>
    public event Action<UpdatelEventArgs>? OnInstallProgress;

    /// <summary>
    /// 更新错误
    /// </summary>
    public event Action<string>? OnError;

    public async Task Start(string url)
    {
        try
        {
            Uri uri = new(url);
            _filename = HttpUtility.UrlDecode(uri.Segments.Last());

            await Download(url);
            _ = Task.Run(Install);
        }
        catch (Exception ex)
        {
            OnError?.Invoke(ex.Message);
            Log.Error(ex, "更新失败");
        }
    }

    public void Cancel()
    {
        _cts.Cancel();
    }

    private async Task Download(string url)
    {
        File.Delete(_zipPath);
        OnDownloadProgress?.Invoke(new UpdatelEventArgs
        {
            Description = $"开始下载文件：{_filename}",
            ProgressValue = 0
        });
        using var resp = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, _cts.Token);
        resp.EnsureSuccessStatusCode();
        using var stream = await resp.Content.ReadAsStreamAsync();
        using var fileStream = File.Create(_zipPath);
        long total = resp.Content.Headers.ContentLength ?? -1;
        long count = 0;
        int len;
        byte[] buffer = new byte[1024];
        while ((len = await stream.ReadAsync(buffer)) > 0)
        {
            count += len;
            if (_cts.IsCancellationRequested)
            {
                break;
            }

            if (total > 0)
            {
                OnDownloadProgress?.Invoke(new UpdatelEventArgs
                {
                    Description = $"{ToMebibyte(count)}M/{ToMebibyte(total)}M",
                    ProgressValue = (int)(count * 1d / total * 100)
                });
            }
            await fileStream.WriteAsync(buffer.AsMemory(0, len));
        }

        static double ToMebibyte(long num) => Math.Round(num * 1d / 1024 / 1024, 2);
    }

    private void Install()
    {
        var archive = ZipFile.Open(_zipPath, ZipArchiveMode.Read, System.Text.Encoding.Default);
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
            count++;

            if (entry.Length == 0)
            {
                continue;
            }
            var filename = entry.FullName;          
            entry.ExtractToFile(Path.Combine(_destinationPath, filename), true);
        }
        archive.Dispose();
        File.Delete(_zipPath);
    }

}

public sealed class UpdatelEventArgs
{
    public string Description { get; set; } = string.Empty;
    public int ProgressValue { get; set; }
}
