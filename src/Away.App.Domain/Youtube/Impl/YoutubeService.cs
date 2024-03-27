using Away.App.Domain.Youtube.Models;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Away.App.Domain.Youtube.Impl;

[DI(ServiceLifetime.Singleton)]
public class YoutubeService(IHttpClientFactory httpClientFactory, IYoutubeRepository youtubeRepository) : IYoutubeService
{
    private YoutubeClient? _youtube;
    private HttpClient? _httpClient;

    private HttpClient HttpClient => _httpClient ??= httpClientFactory.CreateClient();
    private YoutubeClient YoutubeClient => _youtube ??= new YoutubeClient(HttpClient);
    private string RootPath => youtubeRepository.GetSetting("RootPath").Value;

    public ValueTask<Video> GetVideoInfo(string url)
    {
        return YoutubeClient.Videos.GetAsync(url);
    }

    public async ValueTask<List<FileModel>> DownloadThumbnails(Video video)
    {
        var list = new List<FileModel>();
        foreach (var item in video.Thumbnails)
        {
            var res = await DownloadImage(item.Url, o =>
            {
                o.AddRelativePath(video.Id);
                o.FileName = $"{item.Resolution}";
                o.FileExtension = ".jpg";
            });
            list.Add(res);
        }
        return list;
    }

    public async ValueTask<FileModel> DownloadThumbnail(Video video, string area = "1920x1080")
    {
        var item = video.Thumbnails.FirstOrDefault(o => o.Resolution.ToString() == area);
        item ??= video.Thumbnails.OrderByDescending(o => o.Resolution.ToString()).FirstOrDefault()!;
        var res = await DownloadImage(item.Url, o =>
        {
            o.AddRelativePath(video.Id);
            o.FileName = $"{item.Resolution}";
            o.FileExtension = ".jpg";
        });
        return res;
    }

    private async Task<FileModel> DownloadImage(string url, Action<DownloadBuilder>? action = null)
    {
        var builder = new DownloadBuilder
        {
            RootPath = RootPath,
            FileExtension = Path.GetExtension(url)
        };
        action?.Invoke(builder);
        var fileRes = builder.Build();

        if (File.Exists(fileRes.FileRootPath))
        {
            return fileRes;
        }

        byte[] buffer = await HttpClient.GetByteArrayAsync(url);
        using var fs = new FileStream(fileRes.FileRootPath, FileMode.CreateNew);
        fs.Write(buffer, 0, buffer.Length);
        return fileRes;
    }

    public async ValueTask<FileModel> DownloadVideo(Video video, Action<DownloadBuilder>? action = null)
    {
        var builder = new DownloadBuilder
        {
            RootPath = RootPath,
            FileName = video.Id.ToString(),
            FileExtension = ".mp4"
        };
        builder.AddRelativePath(video.Id);
        action?.Invoke(builder);
        var fileRes = builder.Build();
        if (File.Exists(fileRes.FileRootPath))
        {
            return fileRes;
        }

        var streamManifest = await YoutubeClient.Videos.Streams.GetManifestAsync(video.Url);
        var res = streamManifest.GetMuxedStreams().OrderByDescending(o => o.Size).FirstOrDefault();
        var streamInfos = new IStreamInfo[] { res! };

        var request = new ConversionRequestBuilder(fileRes.FileRootPath);
        await YoutubeClient.Videos.DownloadAsync(streamInfos, request.Build());
        return fileRes;
    }
}
