using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectPilotLite.Wpf.Common
{
    /// <summary>
    /// ICommand générique réutilisable pour tout le client WPF (MVVM).
    /// Deux constructeurs pour rester compatible avec tous les écrans du groupe :
    /// - Action&lt;object?&gt; / Predicate&lt;object?&gt; (Tâches, Livrables, Tableau de bord)
    /// - Func&lt;Task&gt; / Func&lt;bool&gt; sans paramètre (Projets)
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?>? _execute;
        private readonly Predicate<object?>? _canExecute;

        private readonly Func<Task>? _executeAsync;
        private readonly Func<bool>? _canExecuteAsync;
        private bool _isRunning;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _executeAsync = execute;
            _canExecuteAsync = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            if (_executeAsync is not null)
            {
                return !_isRunning && (_canExecuteAsync?.Invoke() ?? true);
            }

            return _canExecute?.Invoke(parameter) ?? true;
        }

        public async void Execute(object? parameter)
        {
            if (_executeAsync is not null)
            {
                _isRunning = true;
                CommandManager.InvalidateRequerySuggested();
                try
                {
                    await _executeAsync();
                }
                finally
                {
                    _isRunning = false;
                    CommandManager.InvalidateRequerySuggested();
                }

                return;
            }

            _execute?.Invoke(parameter);
        }
    }
}
