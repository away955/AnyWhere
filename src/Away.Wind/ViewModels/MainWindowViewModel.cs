using System.Collections.ObjectModel;

namespace Away.Wind.ViewModels;

public class MainWindowViewModel : BindableBase
{
    private readonly IRegionManager _regionManager;

    private string _title = "Prism Unity Application";
    public string Title
    {
        get { return _title; }
        set { SetProperty(ref _title, value); }
    }

    public DelegateCommand<string> NavigateCommand { get; private set; }

    public MainWindowViewModel(IRegionManager regionManager)
    {
        _regionManager = regionManager;
        NavigateCommand = new DelegateCommand<string>(Navigate);
    }

    #region Menu Settings



    private ObservableCollection<MenuModel> _leftMenus = [
        new MenuModel
        {
            Icon = "pack://application:,,,/Away.Wind;component/Assets/img_home.png",
            Title = "home",
            URL = "404"
        },
        new MenuModel
        {
            Icon = "pack://application:,,,/Away.Wind;component/Assets/img_contact.png",
            Title = "contact",
            URL = "22"
        },
        new MenuModel
        {
            Icon = "pack://application:,,,/Away.Wind;component/Assets/img_message.png",
            Title = "message",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "pack://application:,,,/Away.Wind;component/Assets/img_map.png",
            Title = "map",
            URL = "settings"
        },
    ];
    public ObservableCollection<MenuModel> LeftMenus { get => _leftMenus; set => SetProperty(ref _leftMenus, value); }

    private ObservableCollection<MenuModel> _topMenus = [
        new MenuModel
        {
            Icon = "pack://application:,,,/Away.Wind;component/Assets/img_setting.png",
            Title = "settings",
            URL = "settings"
        },
        new MenuModel
        {
            Icon = "pack://application:,,,/Away.Wind;component/Assets/img_signout.png",
            Title = "signout",
            URL = "settings"
        }
    ];
    public ObservableCollection<MenuModel> TopMenus { get => _topMenus; set => SetProperty(ref _topMenus, value); }

    #endregion

    private string _logoTitle = "Away DeskTop";
    public string LogoTitle { get => _logoTitle; set => SetProperty(ref _logoTitle, value); }

    private void Navigate(string navigatePath)
    {
        if (navigatePath == null)
        {
            return;
        }
        _regionManager.RequestNavigate("ContentRegion", navigatePath);
    }
}

public class MenuModel
{
    public required string Title { get; set; }
    public required string Icon { get; set; }
    public required string URL { get; set; }

    public ObservableCollection<MenuModel>? Children { get; set; }
}
