namespace Youtube.Views;

public partial class YoutubeView : ReactiveUserControl<YoutubeViewModel>, IView
{
    public YoutubeView()
    {
        ViewModel = AwayLocator.GetService<YoutubeViewModel>();
        InitializeComponent();
    }
}