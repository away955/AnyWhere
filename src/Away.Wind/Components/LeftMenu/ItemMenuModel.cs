namespace Away.Wind.Components;

public class ItemMenuModel : BindableBase
{
    private string _title = string.Empty;
    private string _icon = string.Empty;
    private string _url = string.Empty;

    public required string Title { get => _title; set => SetProperty(ref _title, value); }
    public required string Icon { get => _icon; set => SetProperty(ref _icon, value); }
    public required string URL { get => _url; set => SetProperty(ref _url, value); }
}
