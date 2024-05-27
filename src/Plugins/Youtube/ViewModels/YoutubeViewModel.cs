
namespace Youtube.ViewModels;

public sealed class YoutubeViewModel : ViewModelBase
{
    private readonly IYoutubeMapper _mapper;
    private readonly IYoutubeService _youtubeService;
    private readonly IYoutubeFactory _youtubeFactory;

    /// <summary>
    /// 添加视频
    /// </summary>
    public ICommand AddCommand { get; }

    [Reactive]
    public ObservableCollection<YoutubeModel> Items { get; set; } = [];

    public YoutubeViewModel(
        IYoutubeMapper mapper,
        IYoutubeService youtubeService,
        IYoutubeFactory youtubeFactory)
    {
        _mapper = mapper;
        _youtubeService = youtubeService;
        _youtubeFactory = youtubeFactory;

        _youtubeFactory.DownloadProgress += OnDownloadProgress;

        AddCommand = ReactiveCommand.Create(OnAddCommand);
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

        var list = _youtubeService.GetList().Select(o =>
        {
            var item = _mapper.Map<YoutubeModel>(o);
            item.DelCommand = ReactiveCommand.Create<YoutubeModel>(OnDel);
            item.DownloadCommand = ReactiveCommand.Create<YoutubeModel>(OnDownload);
            item.CancelCommand = ReactiveCommand.Create<YoutubeModel>(OnCancel);
            item.OpenFolderCommand = ReactiveCommand.Create<YoutubeModel>(OnOpenFolder);
            item.InfoCommand = ReactiveCommand.Create<YoutubeModel>(OnInfo);
            return item;
        }).OrderByDescending(o => o.Id);

        Items = new(list);
    }



    private void OnAddCommand()
    {
        ViewRouter.Go("youtube-add");
    }

    private void OnDel(YoutubeModel model)
    {
        var entity = _mapper.Map<YoutubeEntity>(model);
        var res = _youtubeService.Remove(entity);
        if (res)
        {
            MessageShow.Success("删除成功");
            Items.Remove(model);
        }
    }
    private void OnDownload(YoutubeModel model)
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
    private void OnCancel(YoutubeModel model)
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
    private void OnOpenFolder(YoutubeModel model)
    {
        var entity = _mapper.Map<YoutubeEntity>(model);
        var folderPath = _youtubeService.GetFolderPath(entity);
        System.Diagnostics.Process.Start("explorer", folderPath);
    }
    private void OnInfo(YoutubeModel model)
    {
        ViewRouter.Go("youtube-add", model.Id);
    }
}
