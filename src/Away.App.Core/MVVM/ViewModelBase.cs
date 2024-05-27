namespace Away.App.Core.MVVM;


public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    public ViewModelActivator Activator { get; }

    public ViewModelBase()
    {
        Activator = new ViewModelActivator();
        this.WhenActivated(disposables =>
        {
            OnActivation();
            Disposable.Create(OnDeactivation).DisposeWith(disposables);
        });
    }

    /// <summary>
    /// 页面传参
    /// </summary>
    /// <param name="parameter">参数对象</param>
    public virtual void OnParameter(ViewParameter parameter)
    {
    }

    /// <summary>
    /// 进入页面
    /// </summary>
    protected virtual void OnActivation()
    {

    }

    /// <summary>
    /// 离开页面
    /// </summary>
    protected virtual void OnDeactivation()
    {

    }


}
