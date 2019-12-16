using System;
using System.Windows.Input;

namespace Tools
{
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> execute) : base(execute)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) : base(execute, canExecute)
        {
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute = null;

        private readonly Predicate<T> _canExecute = null;

        public RelayCommand(Action<T> execute) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T) parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T) parameter);
        }

        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}