namespace Away.App.Views;

[View("youtube")]
public partial class YoutubeView : ReactiveUserControl<YoutubeViewModel>, IView
{
    public YoutubeView()
    {
        ViewModel = AwayLocator.GetViewModel<YoutubeViewModel>();
        InitializeComponent();
    }
}