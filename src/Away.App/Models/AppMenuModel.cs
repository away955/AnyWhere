namespace Away.App.Models;

public sealed class AppMenuModel : ReactiveObject
{
    private string _icon = string.Empty;
    private string _module = string.Empty;

    /// <summary>
    /// 菜单地址
    /// </summary>
    [Reactive]
    public string Path { get; set; } = string.Empty;
    /// <summary>
    /// 菜单标题
    /// </summary>
    [Reactive]
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// 菜单图标
    /// </summary>
    public string Icon
    {
        get => _icon;
        set
        {
            this.RaiseAndSetIfChanged(ref _icon, value);
            this.RaisePropertyChanged(nameof(IconText));
        }
    }

    public string Module
    {
        get => _module;
        set
        {
            this.RaiseAndSetIfChanged(ref _module, value);
            this.RaisePropertyChanged(nameof(FontFamily));
        }
    }
    /// <summary>
    /// Unicode图标
    /// </summary>
    public string IconText => Icon.ToUnicode();
    /// <summary>
    /// 字体资源
    /// </summary>
    public FontFamily FontFamily => new($"avares://{Module}/Assets/#iconfont");
}
