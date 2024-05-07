using System.Net;

namespace Youtube.Services.Impl;

public sealed class YoutubeFactory : IYoutubeFactory
{
    public event Action<int, double>? DownloadProgress;
    public IWebProxy? Proxy { get; set; }

    private readonly ConcurrentDictionary<int, YoutubeClient> _tasks;
    private readonly IYoutubeRepository _youtubeRepository;

    public YoutubeFactory(IYoutubeRepository youtubeRepository)
    {
        _tasks = new();
        _youtubeRepository = youtubeRepository;
    }

    public bool Cancel(int id)
    {
        if (!_tasks.TryGetValue(id, out var task))
        {
            return false;
        }
        task.Cancel();
        _tasks.TryRemove(id, out var _);
        return _youtubeRepository.SetState(id, YoutubeVideoState.Waiting);
    }

    public bool AddTask(int id)
    {
        if (_tasks.TryGetValue(id, out _))
        {
            Log.Warning($"YoutubeFactory 下载任务已存在：{id}");
            return true;
        }
        var entity = _youtubeRepository.GetById(id);
        if (entity == null)
        {
            return false;
        }

        _youtubeRepository.SetState(id, YoutubeVideoState.Downloading);

        var youtubeClient = new YoutubeClient(entity.Source, Proxy);
        youtubeClient.VideoDownloadProgress += DownloadProgress;
        youtubeClient.VideoDownloadFinished += OnVideoDownloadFinished;

        var res = youtubeClient.DownloadVideo(entity.Id, entity.Source, entity.VideoArea);
        entity.VideoPath = res.FileRootPath;
        entity.State = YoutubeVideoState.Downloading;
        _youtubeRepository.InsertOrUpdate(entity);

        _tasks.TryAdd(id, youtubeClient);
        return true;

        void OnVideoDownloadFinished(bool isFinished, string message)
        {
            _tasks.TryRemove(id, out var _);
            Log.Information($"YoutubeFactory下载结果：{id}\n{message}");
            var state = isFinished ? YoutubeVideoState.Success : YoutubeVideoState.Error;
            _youtubeRepository.SetState(id, state);
        }
    }
}
