using System.Runtime.InteropServices;

namespace Away.Service.Windows;

public static partial class ConsoleManager
{
    private const string Kernel32DllName = "kernel32.dll";

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindow(IntPtr hWnd, int nCmdShow);

    /// <summary>
    /// 打开
    /// </summary>
    /// <returns></returns>
    [LibraryImport(Kernel32DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();

    /// <summary>
    /// 关闭
    /// </summary>
    /// <returns></returns>
    [LibraryImport(Kernel32DllName)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool FreeConsole();

    [LibraryImport(Kernel32DllName)]
    private static partial IntPtr GetConsoleWindow();

    public static bool IsShow { get; private set; }

    public static void Init()
    {
        var handler = GetConsoleWindow();
        if (handler == IntPtr.Zero)
        {
            AllocConsole();
            IsShow = true;
        }
        Hide();
    }

    public static void Show()
    {
        var handler = GetConsoleWindow();
        if (!IsShow)
        {
            ShowWindow(handler, 5);
            IsShow = true;
        }
    }
    public static void Hide()
    {
        var handler = GetConsoleWindow();
        if (IsShow)
        {
            ShowWindow(handler, 0);
            IsShow = false;
        }
    }
    public static void Toggle()
    {
        if (IsShow)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}
