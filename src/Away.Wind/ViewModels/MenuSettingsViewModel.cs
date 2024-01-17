using System.Collections.ObjectModel;

namespace Away.Wind.ViewModels;

public class MenuSettingsViewModel : BindableBase
{
    private string _url = "xxx";
    public string URL { get => _url; set => SetProperty(ref _url, value); }


    private ObservableCollection<MenuEntity> _menuList = [
        new MenuEntity()
        {
            Id = 1,
            Description = "测试1",
            Title = "bbb",
            Url = "http://www.bbb.com"
        },
        new MenuEntity()
        {
            Id = 2,
            Description = "测试2",
            Title = "aaaa",
            Url = "http://www.aaa.com"
        }
    ];
    public ObservableCollection<MenuEntity> MenuList { get => _menuList; set => SetProperty(ref _menuList, value); }

    private int _totalPage = 10;
    public int TotalPage { get => _totalPage; set => SetProperty(ref _totalPage, value); }

    public DelegateCommand<string> QueryCommand { get; private set; }
    public DelegateCommand ResetCommand { get; private set; }
    public DelegateCommand<string> AddCommand { get; private set; }
    public DelegateCommand<string> EditCommand { get; private set; }
    public DelegateCommand<string> DeleteCommand { get; private set; }

    public MenuSettingsViewModel()
    {
        ResetCommand = new DelegateCommand(() =>
        {
            TotalPage += 10;
            MessageBox.Show("rest");
        });
    }
}


public class MenuEntity
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }
}