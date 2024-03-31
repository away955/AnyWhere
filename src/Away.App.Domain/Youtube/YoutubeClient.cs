using Avalonia.Media.Imaging;
using Away.App.Domain.Youtube.Models;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Away.App.Domain.Youtube;

public sealed class YoutubeClient
{
    public event Action<int, double>? VideoDownloadProgress;
    public event Action<bool, string>? VideoDownloadFinished;

    private readonly string _rootPath;
    private readonly HttpClient _httpClient;
    private readonly YoutubeExplode.YoutubeClient _youtube;
    private readonly VideoId _videoId;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public YoutubeClient(string url)
    {
        _httpClient ??= AwayLocator.GetService<IHttpClientFactory>().CreateClient("xray-proxy");
        _videoId = ParseVideoId(url);
        _rootPath = Path.Combine(Environment.CurrentDirectory, "Data", "Videos");
        _youtube ??= new(_httpClient);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();
    }

    public async ValueTask<Video?> GetVideo()
    {
        try
        {
            return await _youtube.Videos.GetAsync(_videoId, _cancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "获取Youtube视频信息失败");
            return null;
        }
    }

    public static VideoId ParseVideoId(string url)
    {
        return VideoId.Parse(url);
    }

    public static async ValueTask<Bitmap?> GetThumbnail(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        try
        {
            var buffer = await File.ReadAllBytesAsync(path);
            return new Bitmap(new MemoryStream(buffer));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "获取Youtube缩列图失败");
            return null;
        }
    }

    public async ValueTask<IEnumerable<IVideoStreamInfo>?> GetMuxedStreams(string url)
    {
        try
        {
            var streamManifest = await GetManifestAsync(url);
            return GetVideoStreamList(streamManifest);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "获取Youtube视频失败");
            return null;
        }
    }

    public async ValueTask<FileModel?> DownloadImage(string url)
    {
        var builder = new DownloadBuilder
        {
            FileName = "封面",
            RootPath = _rootPath,
            FileExtension = ".png"
        };
        builder.AddRelativePath(_videoId);
        var fileRes = builder.Build();
        if (File.Exists(fileRes.FileRootPath))
        {
            return fileRes;
        }

        try
        {
            var buffer = await _httpClient.GetByteArrayAsync(url, _cancellationTokenSource.Token);
            using var fs = new FileStream(fileRes.FileRootPath, FileMode.CreateNew);
            fs.Write(buffer, 0, buffer.Length);
            return fileRes;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "下载封面失败");
            fileRes.DeleteFolder();
            return null;
        }
    }

    public FileModel DownloadVideo(int id, string url, string videoArea)
    {
        var builder = new DownloadBuilder
        {
            RootPath = _rootPath,
            FileName = "视频",
            FileExtension = ".mp4"
        };
        builder.AddRelativePath(_videoId);
        var fileRes = builder.Build();
        if (File.Exists(fileRes.FileRootPath))
        {
            return fileRes;
        }

        _ = Task.Run(async () =>
        {
            try
            {
                var streamManifest = await GetManifestAsync(url);
                var videos = GetVideoStreamList(streamManifest);
                var video = videos.FirstOrDefault(o => o.VideoResolution.ToString() == videoArea)!;
                var audio = streamManifest.GetAudioStreams().OrderByDescending(t => t.Size).FirstOrDefault()!;

                var streamInfos = new IStreamInfo[] { video, audio };
                var request = new ConversionRequestBuilder(fileRes.FileRootPath);
                await _youtube.Videos.DownloadAsync(streamInfos, request.Build(), new DownLoadProgress(this, id), _cancellationTokenSource.Token);
                VideoDownloadFinished?.Invoke(true, "finished");
            }
            catch (Exception ex)
            {
                VideoDownloadFinished?.Invoke(false, ex.Message);
            }
        });
        return fileRes;
    }

    private IEnumerable<IVideoStreamInfo> GetVideoStreamList(StreamManifest streamManifest)
    {
        return streamManifest
                  .GetVideoStreams()
                  .Where(o => o.Container.Name == "mp4")
                  .GroupBy(o => o.VideoResolution)
                  .Select(o => o.OrderByDescending(t => t.Size).FirstOrDefault()!);
    }

    private ValueTask<StreamManifest> GetManifestAsync(string url)
    {
        return _youtube.Videos.Streams.GetManifestAsync(url, _cancellationTokenSource.Token);
    }


    private class DownLoadProgress(YoutubeClient youtubeClient, int id) : IProgress<double>
    {
        public void Report(double value)
        {
            youtubeClient.VideoDownloadProgress?.Invoke(id, Math.Round(value, 2) * 100);
        }
    }
}
