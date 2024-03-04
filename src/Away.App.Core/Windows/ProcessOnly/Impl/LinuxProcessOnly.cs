namespace Away.App.Core.Windows.ProcessOnly.Impl;

public sealed class LinuxProcessOnly : IProcessOnly
{
    public bool Show(string mutexName, bool isShow = true)
    {
        return false;
    }
}
