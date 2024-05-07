namespace Youtube.ViewModels;

public sealed class YoutubeViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IYoutubeService _youtubeService;
    private readonly IYoutubeFactory _youtubeFactory;

    /// <summary>
    /// 添加视频
    /// </summary>
    public ICommand AddCommand { get; }
    /// <summary>
    /// 删除视频
    /// </summary>
    public ICommand DelCommand { get; }
    /// <summary>
    /// 下载视频
    /// </summary>
    public ICommand DownloadCommand { get; }
    /// <summary>
    /// 取消下载
    /// </summary>
    public ICommand CancelCommand { get; }
    /// <summary>
    /// 打开文件夹
    /// </summary>
    public ICommand OpenFolderCommand { get; }

    [Reactive]
    public ObservableCollection<YoutubeModel> Items { get; set; } = [];

    public YoutubeViewModel(
        IMapper mapper,
        IYoutubeService youtubeService,
        IYoutubeFactory youtubeFactory)
    {
        _mapper = mapper;
        _youtubeService = youtubeService;
        _youtubeFactory = youtubeFactory;

        _youtubeFactory.DownloadProgress += OnDownloadProgress;

        AddCommand = ReactiveCommand.Create(OnAddCommand);
        DelCommand = ReactiveCommand.Create<YoutubeModel>(OnDelCommand);
        DownloadCommand = ReactiveCommand.Create<YoutubeModel>(OnDownloadCommand);
        CancelCommand = ReactiveCommand.Create<YoutubeModel>(OnCancelCommand);
        OpenFolderCommand = ReactiveCommand.Create<YoutubeModel>(OnOpenFolderCommand);
    }

    private void OnDownloadProgress(int id, double value)
    {
        var model = Items.FirstOrDefault(o => o.Id == id);
        if (model == null)
        {
            return;
        }
        model.ProgressBarValue = value;
        if (value == 100)
        {
            model.State = YoutubeVideoState.Success;
        }
    }

    protected override void OnActivation()
    {
        Init();
    }

    private void Init()
    {
        var list = _youtubeService.GetList();
        Items = new(list.Select(_mapper.Map<YoutubeModel>).OrderByDescending(o => o.Id));
    }

    private void OnAddCommand()
    {
        MessageRouter.Go("youtube-add");
    }

    private void OnDelCommand(YoutubeModel model)
    {
        var entity = _mapper.Map<YoutubeEntity>(model);
        var res = _youtubeService.Remove(entity);
        if (res)
        {
            MessageShow.Success("删除成功");
            Items.Remove(model);
        }
    }
    private void OnDownloadCommand(YoutubeModel model)
    {
        var flag = _youtubeFactory.AddTask(model.Id);
        if (flag)
        {
            model.State = YoutubeVideoState.Downloading;
            MessageShow.Success("开始下载", model.TitileShort);
        }
        else
        {
            MessageShow.Error("下载失败");
        }
    }
    private void OnCancelCommand(YoutubeModel model)
    {
        var flag = _youtubeFactory.Cancel(model.Id);
        if (flag)
        {
            model.State = YoutubeVideoState.Waiting;
            MessageShow.Success("取消成功", model.TitileShort);
        }
        else
        {
            MessageShow.Error("取消失败");
        }
    }
    private void OnOpenFolderCommand(YoutubeModel model)
    {
        var entity = _mapper.Map<YoutubeEntity>(model);
        var folderPath = _youtubeService.GetFolderPath(entity);
        System.Diagnostics.Process.Start("explorer", folderPath);
    }
}
