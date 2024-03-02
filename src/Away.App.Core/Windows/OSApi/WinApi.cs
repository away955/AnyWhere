using System.Runtime.InteropServices;

namespace Away.App.Core.Windows;

public static partial class WinApi
{
    private const string Kernel32 = "kernel32.dll";
    private const string User32 = "user32.dll";

    [LibraryImport(User32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

    /// <summary>
    /// 打开
    /// </summary>
    /// <returns></returns>
    [LibraryImport(Kernel32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AllocConsole();

    /// <summary>
    /// 关闭
    /// </summary>
    /// <returns></returns>
    [LibraryImport(Kernel32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FreeConsole();

    [LibraryImport(Kernel32)]
    public static partial IntPtr GetConsoleWindow();


    /// <summary>
    ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
    ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
    /// </summary>
    /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
    /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
    [LibraryImport(User32, EntryPoint = "SetForegroundWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// 激活指定窗口
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="fAltTab">是否使最小化的窗口还原</param>
    [LibraryImport(User32)]
    public static partial void SwitchToThisWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool fAltTab);

    /// <summary>
    /// 根据窗口标题查找窗体
    /// </summary>
    /// <param name="lpClassName"></param>
    /// <param name="lpWindowName"></param>
    /// <returns></returns>
    [LibraryImport(User32, EntryPoint = "FindWindow", StringMarshalling = StringMarshalling.Utf16)]
    public static partial IntPtr FindWindow(string lpClassName, string lpWindowName);
}
