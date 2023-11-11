using System;

namespace ProcessListWPF.Commands;

public class RelayCommand : CommandBase
{
    private readonly Predicate<object?>? _canExecutePredicate;
    private readonly Action<object?> _executeAction;

    public RelayCommand(Action<object?> executeAction, Predicate<object?>? canExecutePredicate = null)
    {
        _executeAction = executeAction;
        _canExecutePredicate = canExecutePredicate;
    }

    public override void Execute(object? parameter)
    {
        _executeAction(parameter);
    }

    public override bool CanExecute(object? parameter)
    {
        return _canExecutePredicate == null || _canExecutePredicate(parameter);
    }
}
