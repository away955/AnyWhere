namespace Away.App.Services.Impl;

public sealed class AppThemeService(IAppSettingService repository) : IAppThemeService
{
    private const string THEME = "theme";
    private static readonly Type _type = typeof(ThemeType);
    public ThemeType Get()
    {
        var val = repository.Get(THEME);
        if (string.IsNullOrWhiteSpace(val))
        {
            return ThemeType.Default;
        }
        return (ThemeType)Enum.Parse(_type, val);
    }

    public bool Set(ThemeType type)
    {
        MessageEvent.Run(type, THEME);
        return repository.Set(THEME, type.ToString());
    }

    public void Listen(Action<ThemeType> action)
    {
        MessageEvent.Listen(args => action((ThemeType)args), THEME);
    }
}
