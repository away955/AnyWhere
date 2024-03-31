namespace Away.App.Views;

[View("youtube-add")]
public partial class YoutubeAddView : ReactiveUserControl<YoutubeAddViewModel>, IView
{
    public YoutubeAddView()
    {
        ViewModel = AwayLocator.GetViewModel<YoutubeAddViewModel>();
        InitializeComponent();
    }
}