namespace Away.Wind.Models;

public class XrayDnsModel : BindableBase
{
    private bool _disableCache;
    private bool _disableFallback;

    /// <summary>
    /// DNS 服务器列表
    /// </summary>
    public List<string> servers { get; set; } = [];
    /// <summary>
    /// 禁用 DNS 缓存
    /// </summary>
    public bool disableCache
    {
        get => _disableCache;
        set => SetProperty(ref _disableCache, value);
    }
    /// <summary>
    /// 禁用 DNS 回退
    /// </summary>
    public bool disableFallback
    {
        get => _disableFallback;
        set => SetProperty(ref _disableFallback, value);
    }

    private ObservableCollection<StringItem> _items = [];
    public ObservableCollection<StringItem> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }
}
