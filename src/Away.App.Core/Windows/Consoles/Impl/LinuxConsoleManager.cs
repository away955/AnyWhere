namespace Away.App.Core.Windows.Consoles.Impl;

public sealed class LinuxConsoleManager : IConsoleManager
{
    private static bool _isShow;
    public LinuxConsoleManager(bool defaultShow)
    {
        _isShow = defaultShow;
    }

    public void Toggle()
    { 
    }
}
