namespace Away.App.Services;

public enum ThemeType
{
    Default = 0,
    Dark = 1,
    Light = 2,
}


public interface IAppThemeService
{
    ThemeType Get();
    bool Set(ThemeType type);
    void Listen(Action<ThemeType> action);
}
