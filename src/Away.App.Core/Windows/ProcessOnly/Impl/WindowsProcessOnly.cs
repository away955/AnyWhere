using System.Diagnostics;
namespace Away.App.Core.Windows.ProcessOnly.Impl;

public sealed class WindowsProcessOnly : IProcessOnly
{
    private static EventWaitHandle? ProgramStarted { get; set; }

    /// <summary>
    /// 检查是否已启动，启动则顶置
    /// </summary>
    /// <param name="mutexName">进程唯一编号</param>
    /// <param name="isShow">如果存在：是否显示窗口</param>
    /// <returns></returns>
    public bool Show(string mutexName, bool isShow = true)
    {
        ProgramStarted = new EventWaitHandle(false, EventResetMode.AutoReset, mutexName, out var createdNew);
        if (createdNew)
        {
            return false;
        }

        // 顶置已运行程序窗口
        if (isShow)
        {
            ProgramStarted.Set();
            var current = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(current.ProcessName);
            if (processes?.Length == 0)
            {
                return false;
            }

            var process = processes?.FirstOrDefault(o => o.Id != current.Id)!;
            WinApi.ShowWindow(process.MainWindowHandle, 1);
            WinApi.ShowWindow(process.MainWindowHandle, 5);
        }
        return true;
    }

}
