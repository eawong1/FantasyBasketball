using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Commands;
public class RelayCommand : ICommand
{
    private readonly Func<Task>? _executeAsync;
    private readonly Action? _execute;
    private readonly Func<bool>? _canExecute;
    public event EventHandler? CanExecuteChanged;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public RelayCommand(Func<Task> executeAsync, Func<bool>? canExecute = null)
    {
        _executeAsync = executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
        _canExecute = canExecute;
    }


    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public async void Execute(object? parameter) 
    {
        if(_execute != null)
        {
            _execute();
        }
        else if (_executeAsync != null)
        {
            await _executeAsync();
        }
    }

    
}