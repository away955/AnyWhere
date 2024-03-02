namespace Away.App.Core.Windows.Consoles.Impl;

public class WindowsConsoleManager : IConsoleManager
{
    private static bool _isShow;
    public WindowsConsoleManager(bool defaultShow)
    {
        _isShow = defaultShow;
        var handler = WinApi.GetConsoleWindow();
        if (handler == nint.Zero)
        {
            WinApi.AllocConsole();
        }
        if (!_isShow)
        {
            Hide();
        }
    }

    public static void Show()
    {
        var handler = WinApi.GetConsoleWindow();
        if (!_isShow)
        {
            WinApi.ShowWindow(handler, 5);
            _isShow = true;
        }
    }

    public static void Hide()
    {
        var handler = WinApi.GetConsoleWindow();
        WinApi.ShowWindow(handler, 0);
        _isShow = false;

    }

    public void Toggle()
    {
        if (_isShow)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
