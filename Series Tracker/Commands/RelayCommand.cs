using System;
using System.Windows.Input;

namespace Series_Tracker.Commands
{
    public class RelayCommand(Action<object> execute, Func<object, bool> canExecute) : ICommand
    {
        private readonly Action<object> _execute = execute;
        private readonly Func<object, bool> _canExecute = canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, _ => true)
        {
        }
        public bool CanExecute(object parameter) => _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged;
    }
}
