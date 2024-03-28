using Away.App.Domain.Youtube;

namespace Away.App.ViewModels;

[ViewModel]
public sealed class YoutubeViewModel : ViewModelBase
{
    private readonly IMapper _mapper;
    private readonly IYoutubeService _youtubeService;


    [Reactive]
    public ObservableCollection<YoutubeModel> Items { get; set; } = [];

    public YoutubeViewModel(IMapper mapper, IYoutubeService youtubeService)
    {
        _mapper = mapper;
        _youtubeService = youtubeService;
        Init();
    }

    private void Init()
    {
        var list = _youtubeService.GetList();
        Items = new(list.Select(_mapper.Map<YoutubeModel>));
    }

}
