using Avalonia.ReactiveUI;

namespace Away.App.Core.MVVM;

public abstract class View<TViewModel> : ReactiveUserControl<TViewModel>, IView
    where TViewModel : ViewModelBase
{
    public View()
    {
        ViewModel = AwayLocator.GetService<TViewModel>();
    }

    public string? Query { get; set; }

    public void OnParameter(ViewParameter args)
    {
        ViewModel?.OnParameter(args);
    }
}
