namespace Youtube.Views;

public partial class YoutubeAddView : ReactiveUserControl<YoutubeAddViewModel>, IView
{
    public YoutubeAddView()
    {
        ViewModel = AwayLocator.GetService<YoutubeAddViewModel>();
        InitializeComponent();
    }
}