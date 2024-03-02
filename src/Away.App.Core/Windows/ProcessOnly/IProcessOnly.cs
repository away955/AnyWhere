namespace Away.App.Core.Windows.ProcessOnly;

public interface IProcessOnly
{
    /// <summary>
    /// 检查是否已启动，启动则顶置
    /// </summary>
    /// <param name="mutexName">进程唯一编号</param>
    /// <param name="isShow">如果存在：是否显示窗口</param>
    /// <returns></returns>
    bool Show(string mutexName, bool isShow = true);
}
