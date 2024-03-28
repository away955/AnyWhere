using Away.App.Domain.Youtube;

namespace Away.App.ViewModels;

[ViewModel]
public sealed class YoutubeViewModel : ViewModelBase
{
    private readonly IYoutubeService _youtubeService;

    [Reactive]
    public ObservableCollection<YoutubeModel> Items { get; set; } = [];

    public YoutubeViewModel(IYoutubeService youtubeService)
    {
        _youtubeService = youtubeService;
    }

}
