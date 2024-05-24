using System.Reactive.Disposables;

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
