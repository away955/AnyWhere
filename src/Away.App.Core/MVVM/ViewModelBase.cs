using System.Reactive.Disposables;

namespace Away.App.Core.MVVM;


public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel, IScreen
{
    public ViewModelActivator Activator { get; }
    public RoutingState Router { get; }

    public ViewModelBase()
    {
        Router = new RoutingState();
        Activator = new ViewModelActivator();
        this.WhenActivated(disposables =>
        {
            OnActivation();
            Disposable.Create(OnDeactivation).DisposeWith(disposables);
        });
    }

    protected virtual void OnActivation()
    {

    }

    protected virtual void OnDeactivation()
    {

    }
}
