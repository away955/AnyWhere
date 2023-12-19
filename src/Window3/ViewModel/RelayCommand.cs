using System.Windows.Input;

namespace Window3.ViewModel;


public class RelayCommand : ICommand
{
    private readonly Action<object?>? execute;
    private readonly Func<object?, bool>? canExecute;

    public RelayCommand(Action<object?> execute)
    {
        this.execute = execute;
        canExecute = null;
    }

    public RelayCommand(Action<object?> execute, Func<object?, bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter)
    {
        return canExecute == null || CanExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        execute?.Invoke(parameter);
    }
}

