namespace Away.App.Core.MVVM;

public interface IView
{
    /// <summary>
    /// 页面传参
    /// </summary>
    /// <param name="args"></param>
    void OnParameter(ViewParameter args);
}
