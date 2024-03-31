using Away.App.Domain.Youtube;
using Away.App.Domain.Youtube.Entities;
using Away.App.Domain.Youtube.Impl;
using System.Threading.Tasks;

namespace Away.App.ViewModels;

[ViewModel]
public sealed class YoutubeAddViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IYoutubeService _youtubeService;

    [Reactive]
    public YoutubeModel Data { get; set; } = new();
    /// <summary>
    /// 简介
    /// </summary>
    [Reactive]
    public List<string> DescriptionItems { get; set; } = [];
    /// <summary>
    /// 视频格式列表
    /// </summary>
    [Reactive]
    public ObservableCollection<YoutubeVideoModel> StreamItems { get; set; } = [];
    /// <summary>
    /// 选中的视频格式
    /// </summary>
    [Reactive]
    public YoutubeVideoModel? StreamSelected { get; set; }

    /// <summary>
    /// 保存
    /// </summary>
    public ICommand SaveCommand { get; }
    /// <summary>
    /// 加载视频资源
    /// </summary>
    public ICommand LoadCommand { get; }


    public YoutubeAddViewModel(
        IMapper mapper,
        IYoutubeService youtubeService)
    {
        _mapper = mapper;
        _youtubeService = youtubeService;

        SaveCommand = ReactiveCommand.Create(OnSaveCommand);
        LoadCommand = ReactiveCommand.Create(OnLoadCommand);
    }

    private void OnLoadCommand()
    {
        try
        {
            Task.Run(Load);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "加载失败");
            MessageShow.Error("加载失败");
        }
    }

    private async void Load()
    {
        if (string.IsNullOrWhiteSpace(Data.Source))
        {
            MessageShow.Warning("视频地址不能为空");
            return;
        }
        MessageShow.Information("开始加载视频资源...");
        Data = new YoutubeModel { Source = Data.Source };
        StreamItems = [];
        DescriptionItems = [];

        var youtubeClient = new YoutubeClient(Data.Source);
        var video = await youtubeClient.GetVideo();
        if (video == null)
        {
            MessageShow.Error("加载视频资源失败");
            return;
        }
        Data.Author = video.Author.ChannelTitle;
        Data.Title = video.Title;
        Data.Description = video.Description;
        Data.Uploaded = video.UploadDate.LocalDateTime;
        DescriptionItems = [.. Data.Description.Split("\n")];

        var thumbnail = video.Thumbnails.OrderByDescending(o => o.Resolution.Area).FirstOrDefault()!;
        var imageFile = await youtubeClient.DownloadImage(thumbnail.Url);
        if (imageFile == null)
        {
            MessageShow.Error("加载视频资源失败");
            return;
        }
        Data.ImagePath = imageFile.FileRootPath;
        var bitmap = await YoutubeClient.GetThumbnail(imageFile.FileRootPath);
        var muxedStreams = await youtubeClient.GetMuxedStreams(Data.Source);
        if (muxedStreams == null)
        {
            return;
        }
        var streams = muxedStreams
            .OrderByDescending(o => o.VideoResolution.Area)
            .Select(o => new YoutubeVideoModel
            {
                Bitmap = bitmap,
                Url = o.Url,
                Area = o.VideoResolution.ToString(),
                FileExtentsion = o.Container.Name,
                FileSize = Math.Round(o.Size.MegaBytes, 2)
            });
        StreamItems = new(streams);
    }

    private void OnSaveCommand()
    {
        if (StreamSelected == null)
        {
            MessageShow.Warning("请选择视频格式");
            return;
        }
        Data.VideoArea = StreamSelected.Area;
        var entity = _mapper.Map<YoutubeEntity>(Data);
        _youtubeService.Save(entity);
        MessageShow.Success("保存成功", "请回到主页面进行下载");
    }
}
