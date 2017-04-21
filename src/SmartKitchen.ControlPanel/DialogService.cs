using System;
using System.Windows;
using HSR.CloudSolutions.SmartKitchen.Util;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel
{
    public class DialogService : IDialogService
    {
        private readonly Window _window;

        public DialogService(MainWindow window)
        {
            _window = window;
        }

        public void ShowException(Exception exception)
        {
            _window.Dispatcher.Invoke(() =>
            {
                var message = exception.CreateExceptionDialogMessage();
                MessageBox.Show(_window, message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            });
        }
    }
}
