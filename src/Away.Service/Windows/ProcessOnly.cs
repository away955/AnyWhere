﻿using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Away.Service.Windows;

/// <summary>
/// 唯一进程
/// </summary>
public partial class ProcessOnly
{
    ///<summary>
    /// 该函数设置由不同线程产生的窗口的显示状态
    /// </summary>
    /// <param name="hWnd">窗口句柄</param>
    /// <param name="cmdShow">指定窗口如何显示。查看允许值列表，请查阅ShowWindow函数的说明部分</param>
    /// <returns>如果函数原来可见，返回值为非零；如果函数原来被隐藏，返回值为零</returns>
    [LibraryImport("User32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

    /// <summary>
    ///  该函数将创建指定窗口的线程设置到前台，并且激活该窗口。键盘输入转向该窗口，并为用户改各种可视的记号。
    ///  系统给创建前台窗口的线程分配的权限稍高于其他线程。 
    /// </summary>
    /// <param name="hWnd">将被激活并被调入前台的窗口句柄</param>
    /// <returns>如果窗口设入了前台，返回值为非零；如果窗口未被设入前台，返回值为零</returns>
    [LibraryImport("User32.dll", EntryPoint = "SetForegroundWindow")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool SetForegroundWindow(IntPtr hWnd);

    [LibraryImport("user32.dll")]
    private static partial void SwitchToThisWindow(IntPtr hWnd, [MarshalAs(UnmanagedType.Bool)] bool fAltTab);

    private static EventWaitHandle? ProgramStarted { get; set; }
    /// <summary>
    /// 检查是否已启动，启动则顶置
    /// </summary>
    /// <param name="mutexName">进程唯一编号</param>
    /// <param name="isShow">如果存在：是否显示窗口</param>
    /// <returns></returns>
    public static bool Check(string mutexName, bool isShow = true)
    {
        ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, mutexName, out var createdNew);
        if (createdNew)
        {
            return false;
        }

        // 顶置已运行程序窗口
        if (isShow)
        {
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            if (processes?.Length == 0)
            {
                return false;
            }

            var process = processes?.FirstOrDefault()!;
            ShowWindowAsync(process.MainWindowHandle, 1);
            SetForegroundWindow(process.MainWindowHandle);
            SwitchToThisWindow(process.MainWindowHandle, true);
        }
        return true;
    }
}
