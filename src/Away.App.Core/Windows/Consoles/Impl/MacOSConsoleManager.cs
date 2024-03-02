namespace Away.App.Core.Windows.Consoles.Impl;

public sealed class MacOSConsoleManager : IConsoleManager
{
    private static bool _isShow;
    public MacOSConsoleManager(bool defaultShow)
    {
        _isShow = defaultShow;
    }


    public void Toggle()
    { 
    }
}
