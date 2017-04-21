using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HSR.CloudSolutions.SmartKitchen.ControlPanel.Communication;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace HSR.CloudSolutions.SmartKitchen.ControlPanel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private UnityServiceLocator _serviceLocator;
        private IDialogService _dialogService;

        private async void App_OnStartup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();
            container.Setup();

            _serviceLocator = new UnityServiceLocator(container);

            _dialogService = _serviceLocator.GetInstance<IDialogService>();

            var viewModel = _serviceLocator.GetInstance<MainWindowViewModel>();
            await viewModel.InitAsync();

            var view = _serviceLocator.GetInstance<MainWindow>();
            view.DataContext = viewModel;

            MainWindow = view;

            MainWindow.Show();
        }

        private UnityServiceLocator SetupDependencies()
        {
            var container = new UnityContainer();
            container.Setup();
            return new UnityServiceLocator(container);
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _serviceLocator?.Dispose();
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _dialogService.ShowException(e.Exception);
            e.Handled = true;
        }
    }
}
