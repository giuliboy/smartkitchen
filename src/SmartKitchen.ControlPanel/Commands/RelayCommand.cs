using System;
using System.Windows.Input;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _onExecute;
        private readonly Func<object, bool> _canExecute; 

        public RelayCommand(Action<object> onExecute, Func<object, bool> canExecute = null)
        {
            if (onExecute == null)
            {
                throw new ArgumentNullException(nameof(onExecute));
            }
            _onExecute = onExecute;
            _canExecute = canExecute ?? (o => true);
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute(parameter);
            }
            catch
            {
                return false;
            }
        }

        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                return;
            }
            _onExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
